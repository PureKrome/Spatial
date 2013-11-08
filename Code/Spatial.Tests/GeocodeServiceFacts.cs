using System.Collections.Generic;
using FakeItEasy;
using Shouldly;
using Spatial.Core.Models;
using Spatial.Core.Services;
using Spatial.Services.ApiServices.GoogleMaps;
using Spatial.Services.ApiServices.Nominatum;
using Spatial.Services.Geocode;
using Xunit;

namespace Spatial.Tests
{
    public class GeocodeServiceFacts
    {
        public class GeocodeFacts
        {
            [Fact]
            public void GivenAValidQueryAndNominatiumKnowsThisAddress_Geocode_ReturnsACoordinate()
            {
                // Arrange.
                const decimal latitude = -37.1234m;
                const decimal longitude = 144.1234m;

                var fakeNominatumService = A.Fake<IApiService>();
                A.CallTo(() => fakeNominatumService.Geocode(A<string>.Ignored))
                    .Returns(new NominatimResponse
                    {
                        Lat = latitude.ToString(),
                        Lon = longitude.ToString()
                    });

                var apiServices = new List<IApiService>
                {
                    fakeNominatumService
                };

                var geocodeService = new GeocodeService(apiServices);

                // Act.
                var coordinate = geocodeService.Geocode("blah");

                // Assert.
                coordinate.ShouldNotBe(null);
                coordinate.Latitude.ShouldBe(latitude);
                coordinate.Longitude.ShouldBe(longitude);
            }

            [Fact]
            public void GivenAValidQueryAndNominatiumFailsButGoogleKnowsThisAddress_Geocode_ReturnsACoordinate()
            {
                // Arrange.
                const decimal latitude = -37.1234m;
                const decimal longitude = 144.1234m;

                var fakeNominatumApiService = A.Fake<IApiService>();
                A.CallTo(() => fakeNominatumApiService.Geocode(A<string>.Ignored))
                    .Returns(null);

                var fakeGoogleApiService = A.Fake<IApiService>();
                A.CallTo(() => fakeGoogleApiService.Geocode(A<string>.Ignored))
                    .Returns(new GoogleMapsResponse
                    {
                        Results = new List<Result>
                        {
                            new Result
                            {
                                Geometry = new Geometry
                                {
                                    Location = new Location
                                    {
                                        Lat = (double) latitude,
                                        Lng = (double) longitude
                                    }
                                }
                            }
                        }
                    });

                var apiServices = new List<IApiService>
                {
                    fakeNominatumApiService,
                    fakeGoogleApiService
                };

                var geocodeService = new GeocodeService(apiServices);

                // Act.
                Coordinate coordinate = geocodeService.Geocode("blah");

                // Assert.
                coordinate.ShouldNotBe(null);
                coordinate.Latitude.ShouldBe(latitude);
                coordinate.Longitude.ShouldBe(longitude);
            }

            [Fact]
            public void GivenABadLatitudeValueFromAnApiService_Geocode_ReturnsNull()
            {
                // Arrange.
                var fakeNominatumService = A.Fake<IApiService>();
                A.CallTo(() => fakeNominatumService.Geocode(A<string>.Ignored))
                    .Returns(new NominatimResponse
                    {
                        Lat = "-1",
                        Lon = "asdasdsadasdsa"
                    });

                var apiServices = new List<IApiService>
                {
                    fakeNominatumService
                };

                var geocodeService = new GeocodeService(apiServices);

                // Act.
                Coordinate coordinate = geocodeService.Geocode("blah");

                // Assert.
                coordinate.ShouldBe(null);
            }
        }
    }
}