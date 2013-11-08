using System.Collections.Generic;
using Spatial.Core.Models;

namespace Spatial.Services.ApiServices.Nominatum
{
    public class NominatimResponse : ICoordinateCovertable
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