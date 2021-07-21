using Newtonsoft.Json;
using System.Collections.Generic;

namespace Chronos.AzureMaps.ManifestGenerator.Models
{
    public class DwgLayers
    {
        public List<string> exterior { get; set; }
        public List<string> unit { get; set; }
        public List<string> wall { get; set; }
        public List<string> door { get; set; }
        public List<string> furn { get; set; }
        public List<string> unitLabel { get; set; }
        public List<string> zone { get; set; }
        public List<string> zoneLabel { get; set; }
        //[JsonIgnore]
        public List<string> unitCategory { get; set; }
    }
}
