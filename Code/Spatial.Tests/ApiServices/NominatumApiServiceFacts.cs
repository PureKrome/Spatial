using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Shouldly;
using WorldDomination.Net.Http;
using WorldDomination.Spatial.ApiServices.Nominatum;
using Xunit;

namespace Spatial.Tests.ApiServices
{
    public class NominatumApiServiceFacts
    {
        public class GeocodeFacts
        {
            [Fact]
            public async Task GivenAValidQuery_Geocode_ReturnsSomeData()
            {
                // Arrange.
                var json = File.ReadAllText("Sample Data\\Google - Mominatim - Result.json");
                var response = FakeHttpMessageHandler.GetStringHttpResponseMessage(json);
                HttpClientFactory.MessageHandler = new FakeHttpMessageHandler(response);
                var service = new NominatimApiService();

                // Act.
                var result = await service.GeocodeAsync("whatever");

                // Assert.
                result.Count.ShouldBe(2);
                result.First().DisplayName
                    .ShouldBe("Upper Heidelberg Road, Ivanhoe, City of Banyule, Victoria, 3079, Australia");
                result.First().Lat.ShouldBe("-37.7657175");
                result.First().Lon.ShouldBe("145.0461003");
            }

            [Fact]
            public async Task GivenAnInValidQuery_Geocode_ReturnsNoResults()
            {
                // Arrange.
                var json = File.ReadAllText("Sample Data\\Google - Mominatim - No Result.json");
                var response = FakeHttpMessageHandler.GetStringHttpResponseMessage(json);
                HttpClientFactory.MessageHandler = new FakeHttpMessageHandler(response);
                var service = new NominatimApiService();

                // Act.
                var result = await service.GeocodeAsync("sdfhgjshf ashdf ashdfj asd gfajskdg");

                // Assert.
                result.Count.ShouldBe(0);
            }
        }
    }
}