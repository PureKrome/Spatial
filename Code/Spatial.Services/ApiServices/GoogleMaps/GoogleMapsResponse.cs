using System.Collections.Generic;
using Spatial.Core.Models;

namespace Spatial.Services.ApiServices.GoogleMaps
    {
    public class AddressComponent
    {
        public string LongName { get; set; }
        public string ShortName { get; set; }
        public List<string> Types { get; set; }
    }

    public class Northeast
    {
        public double Lat { get; set; }
        public double Lng { get; set; }
    }

    public class Southwest
    {
        public double Lat { get; set; }
        public double Lng { get; set; }
    }

    public class Bounds
    {
        public Northeast Northeast { get; set; }
        public Southwest Southwest { get; set; }
    }

    public class Location
    {
        public double Lat { get; set; }
        public double Lng { get; set; }
    }

    public class Northeast2
    {
        public double Lat { get; set; }
        public double Lng { get; set; }
    }

    public class Southwest2
    {
        public double Lat { get; set; }
        public double Lng { get; set; }
    }

    public class Viewport
    {
        public Northeast2 Northeast { get; set; }
        public Southwest2 Southwest { get; set; }
    }

    public class Geometry
    {
        public Bounds Bounds { get; set; }
        public Location Location { get; set; }
        public string LocationType { get; set; }
        public Viewport Viewport { get; set; }
    }

    public class Result : ICoordinateCovertable
    {
        public List<AddressComponent> AddressComponents { get; set; }
        public string FormattedAddress { get; set; }
        public Geometry Geometry { get; set; }
        public bool PartialMatch { get; set; }
        public List<string> Types { get; set; }

        public Coordinate ToCoordinate
        {
            get
            {
                return Geometry != null &&
                       Geometry.Location != null
                    ? new Coordinate
                    {
                        Latitude = (decimal) Geometry.Location.Lat,
                        Longitude = (decimal) Geometry.Location.Lng
                    }
                    : null;
            }
        }
    }

    public class GoogleMapsResponse : ICoordinateCovertable
    {
        public List<Result> Results { get; set; }
        public string Status { get; set; }
        public string ErrorMessage { get; set; }

        /// <summary>
        ///     NOTE: This will only be the first Result.
        /// </summary>
        public Coordinate ToCoordinate
        {
            get
            {
                return Results != null &&
                       Results.Count > 0
                    ? Results[0].ToCoordinate
                    : null;
            }
        }
    }
}