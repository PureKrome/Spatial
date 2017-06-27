using System.Collections.Generic;
using Newtonsoft.Json;

namespace WorldDomination.Spatial.ApiServices.Nominatum
{
    public class NominatimResponse : ICoordinateCovertable
    {
        [JsonProperty("place_id")]
        public string PlaceId { get; set; }

        public string Licence { get; set; }

        [JsonProperty("osm_type")]
        public string OsmType { get; set; }

        [JsonProperty("osm_id")]
        public string OsmId { get; set; }

        public List<string> Boundingbox { get; set; }
        public string Lat { get; set; }
        public string Lon { get; set; }

        [JsonProperty("display_name")]
        public string DisplayName { get; set; }

        public string Type { get; set; }
        public double Importance { get; set; }

        public Coordinate ToCoordinate
        {
            get
            {
                if (string.IsNullOrWhiteSpace(Lat) ||
                    string.IsNullOrWhiteSpace(Lon))
                {
                    return null;
                }

                decimal latitude;
                decimal longitude;

                if (!decimal.TryParse(Lat, out latitude))
                {
                    return null;
                }

                if (!decimal.TryParse(Lon, out longitude))
                {
                    return null;
                }

                return new Coordinate
                {
                    Latitude = latitude,
                    Longitude = longitude
                };
            }
        }
    }
}