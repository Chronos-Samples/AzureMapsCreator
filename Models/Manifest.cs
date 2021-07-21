using System.Collections.Generic;

namespace Chronos.AzureMaps.ManifestGenerator.Models
{
    public class Manifest
    {
        public string version { get; set; }
        public DirectoryInfo directoryInfo { get; set; }
        public BuildingLevels buildingLevels { get; set; }
        public Georeference georeference { get; set; }
        public DwgLayers dwgLayers { get; set; }
        public List<UnitProperty> unitProperties { get; set; }
        public List<ZoneProperty> zoneProperties { get; set; }
    }
}
