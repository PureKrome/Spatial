using System.Collections.Generic;

namespace Spatial.Services.ApiServices.Nominatum
{
    public class NominatimResponse
    {
        public string PlaceId { get; set; }
        public string Licence { get; set; }
        public string OsmType { get; set; }
        public string OsmId { get; set; }
        public List<string> Boundingbox { get; set; }
        public string Lat { get; set; }
        public string Lon { get; set; }
        public string DisplayName { get; set; }
        //public string class { get; set; }
        public string Type { get; set; }
        public double Importance { get; set; }
    }
}