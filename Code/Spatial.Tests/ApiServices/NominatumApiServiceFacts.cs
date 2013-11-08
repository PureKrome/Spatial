using System.Collections.Generic;
using System.Linq;
using Shouldly;
using Spatial.Services.ApiServices.Nominatum;
using Xunit;

namespace Spatial.Tests.ApiServices
{
    public class NominatumApiServiceFacts
    {
        public class GeocodeFacts
        {
            [Fact(Skip = "Integration Test")]
            public void GivenAValidQuery_Geocode_ReturnsSomeData()
            {
                // Arrange.
                var service = new NominatimApiService();

                // Act.
                var result = service.Geocode("395 upper heidelberg road, ivanhoe, victoria, australia") as IList<NominatimResponse>;

                // Assert.
                result.ShouldNotBe(null);
                result.Count.ShouldBe(1);
                result.First().DisplayName
                    .ShouldBe("Upper Heidelberg Road, Ivanhoe, City of Banyule, Victoria, 3079, Australia");
                result.First().Lat.ShouldBe("-37.7727135");
                result.First().Lon.ShouldBe("145.0407028");
            }

            [Fact(Skip = "Integration Test")]
            public void GivenAnInValidQuery_Geocode_ReturnsANull()
            {
                // Arrange.
                var service = new NominatimApiService();

                // Act.
                var result = service.Geocode("sdfhgjshf ashdf ashdfj asd gfajskdg") as IList<NominatimResponse>;

                // Assert.
                result.ShouldBe(null);
            }
        }
    }
}