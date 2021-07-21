using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

using Newtonsoft.Json;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

using Chronos.AzureMaps.ManifestGenerator.Models;

namespace Chronos.AzureMaps.ManifestGenerator
{
    class ManifestGenerator
    {
        private static Dictionary<string, string> Categories = new Dictionary<string, string> { };
        internal static Manifest LoadDefaultManifest()
        {
            Manifest manifest = null;
            var dir = Directory.GetCurrentDirectory();
            string manifestPath = $"{dir}\\manifest-defaults.json";
            string categoryMappingPath = $"{dir}\\CategoryMapping.json";

            if (File.Exists(manifestPath))
            {
                string manifestDefaults = File.ReadAllText(manifestPath);
                manifest = JsonConvert.DeserializeObject<Manifest>(manifestDefaults);
            }

            if (File.Exists(categoryMappingPath))
            {
                string mappings = File.ReadAllText(categoryMappingPath);
                Categories = JsonConvert.DeserializeObject<Dictionary<string, string>>(mappings);
            }

            return manifest;
        }

        internal static void LoadCADIntoManifest(Manifest manifest, string cadDir)
        {
            string[] cadFiles = Directory.GetFiles(cadDir, "*.dwg");

            int levelOrdinal = 0;
            foreach (var cadFile in cadFiles)
            {
                Console.WriteLine($"Processing {cadFile}");
                using (var img = Aspose.CAD.Image.Load(cadFile))
                {
                    var cadImage = img as CadImage;
                    if (cadImage == null) continue;

                    var cadFileName = Path.GetFileName(cadFile);
                    var levelString = Regex.Match(cadFileName, @"\d+").Value;
                    
                    var level = new Models.Level
                    {
                        levelName = $"Level {levelString}",
                        ordinal = levelOrdinal,
                        filename = $"./{cadFileName}"
                    };
                    manifest.buildingLevels.levels.Add(level);

                    var nameEntities = cadImage.Entities.Where(i => i.LayerName.Contains(manifest.dwgLayers.unitLabel.First())).ToList();
                    var typeEntities = cadImage.Entities.Where(i => i.LayerName.Contains(manifest.dwgLayers.unitCategory.First())).ToList();
                    var unitEntities = cadImage.Entities.Where(i => i.LayerName == manifest.dwgLayers.unit.First()).Select(e => ((CadLwPolyline)e).Coordinates).ToList();

                    HashSet<string> unitsset = new HashSet<string>();
                    foreach (var entity in nameEntities)
                    {
                        var entityText = entity as CadText;

                        if (IsUnitAvailable(unitEntities, entityText.FirstAlignment))
                        {
                            var closestTypeEntity = typeEntities.Aggregate(
                               (te1, te2) => entityText.FirstAlignment.Distance(((CadText)te1).FirstAlignment) < entityText.FirstAlignment.Distance(((CadText)te2).FirstAlignment) ? te1 : te2);

                            var typeText = closestTypeEntity as CadText;
                            if (entityText != null && !unitsset.Contains(entityText.DefaultValue))
                            {
                                unitsset.Add(entityText.DefaultValue);
                                Console.Write($"{entityText.DefaultValue};");
                                manifest.unitProperties.Add(new UnitProperty() { unitName = entityText.DefaultValue, categoryName = GetCategoryName(typeText.DefaultValue) });
                            }
                        }
                        else
                        {
                            Console.WriteLine($"No Unit found for label {entityText.DefaultValue}");
                        }
                    }

                }

                levelOrdinal++;
            }
        }

        private static bool IsUnitAvailable(List<List<Cad2DPoint>> unitEntities, Cad3DPoint point)
        {
            foreach (var unit in unitEntities)
            {
                if (IsInside(unit, point.X, point.Y))
                    return true;
            }

            return false;
        }

        private static bool IsInside(List<Cad2DPoint> polygon, double x, double y)
        {
            int pointsCount = polygon.Count - 1;
            double totalAngle = GetAngle(polygon[pointsCount].X, polygon[pointsCount].Y, x, y, polygon[0].X, polygon[0].Y);

            for (int i = 0; i < pointsCount; i++)
            {
                totalAngle += GetAngle(polygon[i].X, polygon[i].Y, x, y, polygon[i + 1].X, polygon[i + 1].Y);
            }

            return (Math.Abs(totalAngle) > 1);
        }

        private static double GetAngle(double Ax, double Ay, double Bx, double By, double Cx, double Cy)
        {
            double dotProduct = ScalarProduct(Ax, Ay, Bx, By, Cx, Cy);

            double crossProduct = CrossProductLength(Ax, Ay, Bx, By, Cx, Cy);

            return Math.Atan2(crossProduct, dotProduct);
        }

        private static double ScalarProduct(double Ax, double Ay, double Bx, double By, double Cx, double Cy)
        {
            // Get the vectors' coordinates.
            double BAx = Ax - Bx;
            double BAy = Ay - By;
            double BCx = Cx - Bx;
            double BCy = Cy - By;

            // Calculate the scalar product.
            return (BAx * BCx + BAy * BCy);
        }

        private static double CrossProductLength(double Ax, double Ay, double Bx, double By, double Cx, double Cy)
        {
            // Get the vectors' coordinates.
            double BAx = Ax - Bx;
            double BAy = Ay - By;
            double BCx = Cx - Bx;
            double BCy = Cy - By;

            // Calculate the Z coordinate of the cross product.
            return (BAx * BCy - BAy * BCx);
        }

        private static string GetCategoryName(string name)
        {
            string key = new string((from c in name where char.IsLetter(c) select c).ToArray());

            if (Categories.ContainsKey(key))
            {
                return Categories[key];
            }
            return "room.other";
        }
    }
}
