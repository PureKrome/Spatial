using System.Collections.Generic;
using Shouldly;
using Spatial.Core.Models;
using Spatial.Core.Services;

namespace Spatial.Services.Geocode
{
    public class GeocodeService : IGeocodeService
    {
        private readonly IList<IApiService> _apiServices;

        public GeocodeService(IList<IApiService> apiServices)
        {
            apiServices.ShouldNotBe(null);
            apiServices.ShouldNotBeEmpty();

            _apiServices = apiServices;
        }

        public Coordinate Geocode(string query)
        {
            string.IsNullOrWhiteSpace(query).ShouldBe(false);

            Coordinate coordinate = null;

            foreach (IApiService apiService in _apiServices)
            {
                var result = apiService.Geocode(query) as ICoordinateCovertable;
                if (result == null)
                {
                    continue;
                }

                coordinate = result.ToCoordinate;
                break;
            }

            return coordinate;
        }
    }
}