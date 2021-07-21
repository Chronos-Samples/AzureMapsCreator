using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Newtonsoft.Json;

namespace Chronos.AzureMaps.ManifestGenerator
{
    class Program
    {
        private static readonly Dictionary<string, Action<string[]>> commands = new Dictionary<string, Action<string[]>>(StringComparer.InvariantCultureIgnoreCase)
        {
            [nameof(BuildManifest)] = BuildManifest,
            ["-bm"] = BuildManifest,
            ["-h"] = Help
        };

        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Invalid args.");
                Help();
                return;
            }

            var command = args[0];

            if (!commands.ContainsKey(command))
            {
                Console.WriteLine("Invalid Command");
                return;
            }

            commands[command](args.Skip(1).ToArray());
        }

        static void BuildManifest(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Invalid args. Specify path to the folder with DWG files.");
                return;
            }

            string dir = args[0];

            var manifest = ManifestGenerator.LoadDefaultManifest();
            ManifestGenerator.LoadCADIntoManifest(manifest, dir);

            string manifestString = JsonConvert.SerializeObject(manifest, Formatting.Indented, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
            File.WriteAllText($"{dir}\\manifest.json", manifestString);
        }

        static void Help(string[] args = null)
        {
            Console.WriteLine("List of available commands:");
            foreach (var cmd in commands.Keys)
            {
                Console.WriteLine(cmd);
            }
        }

    }
}
