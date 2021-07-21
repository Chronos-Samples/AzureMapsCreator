using System.Collections.Generic;

using Newtonsoft.Json;

namespace Chronos.AzureMaps.ManifestGenerator.Models
{
    public class UnitProperty
    {
        public string unitName { get; set; }
        public string categoryName { get; set; }
        public List<string> navigableBy { get; set; }
        public string routeThroughBehavior { get; set; }
        public List<Occupant> occupants { get; set; }
        public string nameAlt { get; set; }
        public string nameSubtitle { get; set; }
        public string addressRoomNumber { get; set; }
        [JsonIgnore]
        public bool nonPublic { get; set; }
        [JsonIgnore]
        public bool isRoutable { get; set; }
        [JsonIgnore]
        public bool isOpenArea { get; set; }
        public string verticalPenetrationCategory { get; set; }
        public string verticalPenetrationDirection { get; set; }
    }
}
