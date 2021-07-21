using System.Collections.Generic;

namespace Chronos.AzureMaps.ManifestGenerator.Models
{
    public class DirectoryInfo
    {
        public string name { get; set; }
        public string streetAddress { get; set; }
        public string unit { get; set; }
        public string locality { get; set; }
        public string postalCode { get; set; }
        public List<string> adminDivisions { get; set; }
        public string hoursOfOperation { get; set; }
        public string phone { get; set; }
        public string website { get; set; }
        public bool nonPublic { get; set; }
        public double anchorLatitude { get; set; }
        public double anchorLongitude { get; set; }
        public int anchorHeightAboveSeaLevel { get; set; }
        public int defaultLevelVerticalExtent { get; set; }
    }
}
