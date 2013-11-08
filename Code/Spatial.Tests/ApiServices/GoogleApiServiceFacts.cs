using System.Linq;
using Shouldly;
using Spatial.Services.ApiServices.GoogleMaps;
using Xunit;

namespace Spatial.Tests.ApiServices
{
    public class GoogleApiServiceFacts
    {
        public class GeocodeFacts
        {
            [Fact(Skip = "Integration Test")]
            //[Fact]
            public void GivenAValidQuery_Geocode_ReturnsSomeData()
            {
                // Arrange.
                var service = new GoogleMapsApiService();

                // Act.
                var result =
                    service.Geocode("395 upper heidelberg road, ivanhoe, victoria, australia") as GoogleMapsResponse;

                // Assert.
                result.ShouldNotBe(null);
                string.IsNullOrWhiteSpace(result.ErrorMessage).ShouldBe(true);
                result.Results.Count.ShouldBe(1);
                var data = result.Results.First();
                data.ShouldNotBe(null);
                data.FormattedAddress.ShouldBe("395 Upper Heidelberg Road, Ivanhoe VIC 3079, Australia");
                data.Geometry.ShouldNotBe(null);
                data.Geometry.Location.ShouldNotBe(null);
                data.Geometry.Location.Lat.ShouldBe(-37.758124);
                data.Geometry.Location.Lng.ShouldBe(145.05311);
            }

            [Fact(Skip = "Integration Test")]
            public void GivenAnInValidQuery_Geocode_ReturnsANull()
            {
                // Arrange.
                var service = new GoogleMapsApiService();

                // Act.
                var result = service.Geocode("sdfhgjshf ashdf ashdfj asd gfajskdg") as GoogleMapsResponse;

                // Assert.
                result.ShouldBe(null);
            }
        }
    }
}