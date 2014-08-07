using System.IO;
using System.Linq;
using System.Threading.Tasks;
using HttpClient.Helpers;
using Shouldly;
using Spatial.Services.ApiServices.GoogleMaps;
using Xunit;

namespace Spatial.Tests.ApiServices
{
    public class GoogleApiServiceFacts
    {
        public class GeocodeFacts
        {
            [Fact]
            //[Fact]
            public async Task GivenAValidQuery_Geocode_ReturnsSomeData()
            {
                // Arrange.
                var service = new GoogleMapsApiService();

                // Act.
                const string streetNumber = "4";
                const string street = "Albert Pl";
                const string suburb = "RICHMOND";
                const string state = "VIC";
                const string postcode = "3121";
                const string country = "AUSTRALIA";
                var query = string.Format("{0} {1}, {2} {3} {4}, {5}", streetNumber,
                    street,
                    suburb,
                    state,
                    postcode,
                    country);

                var componentFilters = new ComponentFilters
                {
                    PostalCode = postcode
                };
                var json = File.ReadAllText("Sample Data\\Google - Geocode - Results.json");
                var response = FakeHttpMessageHandler.GetStringHttpResponseMessage(json);
                HttpClientFactory.MessageHandler = new FakeHttpMessageHandler(response);

                var result = await service.GeocodeAsync(query, componentFilters);

                // Assert.
                result.ErrorMessage.ShouldBeNullOrEmpty();
                result.Results.Count.ShouldBe(3);
                var data = result.Results.First();
                data.FormattedAddress.ShouldBe("4 Albert Street, Richmond VIC 3121, Australia");
                data.Geometry.Location.Lat.ShouldBe(-37.828601);
                data.Geometry.Location.Lng.ShouldBe(144.997996);
            }

            [Fact]
            public async Task GivenAnInValidQuery_Geocode_ReturnsANull()
            {
                // Arrange.
                var json = File.ReadAllText("Sample Data\\Google - Geocode - Zero Results.json");
                var response = FakeHttpMessageHandler.GetStringHttpResponseMessage(json);
                HttpClientFactory.MessageHandler = new FakeHttpMessageHandler(response);

                var service = new GoogleMapsApiService();

                // Act.
                var result = await service.GeocodeAsync("sdfhgjshf ashdf ashdfj asd gfajskdg");

                // Assert.
                result.Results.Count.ShouldBe(0);
                result.Status.ShouldBe("ZERO_RESULTS");
            }

            [Fact]
            public async Task GivenAGeocodingErrorOccured_Geocode_ReturnsAResultWithTheErrorMessage()
            {
                // Arrange.
                var json = File.ReadAllText("Sample Data\\Google - Geocode - Error Result.json");
                var response = FakeHttpMessageHandler.GetStringHttpResponseMessage(json);
                HttpClientFactory.MessageHandler = new FakeHttpMessageHandler(response);

                var service = new GoogleMapsApiService();

                // Act.
                var result = await service.GeocodeAsync("whatever");

                // Assert.
                result.Results.Count.ShouldBe(0);
                result.Status.ShouldBe("REQUEST_DENIED");
                result.ErrorMessage.ShouldBe(
                    "The 'sensor' parameter specified in the request must be set to either 'true' or 'false'.");
            }
        }
    }
}