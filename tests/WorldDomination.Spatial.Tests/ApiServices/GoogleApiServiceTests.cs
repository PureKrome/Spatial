using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Shouldly;
using WorldDomination.Net.Http;
using WorldDomination.Spatial.ApiServices.GoogleMaps;
using Xunit;
// ReSharper disable ConsiderUsingConfigureAwait

namespace WorldDomination.Spatial.Tests.ApiServices
{
    public class GoogleApiServiceTests
    {
        public class GeocodeTests
        {
            [Fact]
            public async Task GivenAValidQuery_Geocode_ReturnsSomeData()
            {
                // Arrange.
                const string streetNumber = "4";
                const string street = "Albert Pl";
                const string suburb = "RICHMOND";
                const string state = "VIC";
                const string postcode = "3121";
                const string country = "AUSTRALIA";
                var query = $"{streetNumber} {street}, {suburb} {state} {postcode}, {country}";

                var componentFilters = new ComponentFilters
                {
                    PostalCode = postcode
                };
                var json = File.ReadAllText("Sample Data\\Google\\Results.json");
                var response = FakeHttpMessageHandler.GetStringHttpResponseMessage(json);
                var options = new HttpMessageOptions
                {
                    RequestUri = new Uri("https://maps.googleapis.com/maps/api/geocode/json?address=4 Albert Pl%2C RICHMOND VIC 3121%2C AUSTRALIA&sensor=false&key=some api key&components=postal_code:3121"),
                    HttpResponseMessage = response
                };
                var httpClient = new HttpClient(new FakeHttpMessageHandler(options));
                var service = new GoogleMapsApiService("some api key", httpClient);

                // Act.
                var result = await service.GeocodeAsync(query, componentFilters);

                // Assert.
                result.ErrorMessage.ShouldBeNullOrEmpty();
                result.Results.Count.ShouldBe(3);
                
                var data = result.Results.First();
                data.FormattedAddress.ShouldBe("4 Albert Street, Richmond VIC 3121, Australia");
                data.Geometry.Location.Lat.ShouldBe(-37.828601);
                data.Geometry.Location.Lng.ShouldBe(144.997996);
                
                options.NumberOfTimesCalled.ShouldBe(1);
            }

            [Fact]
            public async Task GivenAnInValidQuery_Geocode_ReturnsANull()
            {
                // Arrange.
                var json = File.ReadAllText("Sample Data\\Google\\Zero Results.json");
                var response = FakeHttpMessageHandler.GetStringHttpResponseMessage(json);
                var options = new HttpMessageOptions
                {
                    RequestUri = new Uri("https://maps.googleapis.com/maps/api/geocode/json?address=sdfhgjshf ashdf ashdfj asd gfajskdg&sensor=false&key=some api key"),
                    HttpResponseMessage = response
                };
                var httpClient = new HttpClient(new FakeHttpMessageHandler(options));
                var service = new GoogleMapsApiService("some api key", httpClient);

                // Act.
                var result = await service.GeocodeAsync("sdfhgjshf ashdf ashdfj asd gfajskdg");

                // Assert.
                result.Results.Count.ShouldBe(0);
                result.Status.ShouldBe("ZERO_RESULTS");

                options.NumberOfTimesCalled.ShouldBe(1);
            }

            [Fact]
            public async Task GivenAGeocodingErrorOccured_Geocode_ReturnsAResultWithTheErrorMessage()
            {
                // Arrange.
                var json = File.ReadAllText("Sample Data\\Google\\Error Result.json");
                var response = FakeHttpMessageHandler.GetStringHttpResponseMessage(json);
                var options = new HttpMessageOptions
                {
                    RequestUri = new Uri("https://maps.googleapis.com/maps/api/geocode/json?address=whatever&sensor=false&key=some api key"),
                    HttpResponseMessage = response
                };
                var httpClient = new HttpClient(new FakeHttpMessageHandler(options));
                var service = new GoogleMapsApiService("some api key", httpClient);

                // Act.
                var result = await service.GeocodeAsync("whatever");

                // Assert.
                result.Results.Count.ShouldBe(0);
                result.Status.ShouldBe("REQUEST_DENIED");
                result.ErrorMessage.ShouldBe(
                    "The 'sensor' parameter specified in the request must be set to either 'true' or 'false'.");

                options.NumberOfTimesCalled.ShouldBe(1);
            }

            /// <summary>
            /// This is an example of where google cannot find the location at all. It bubbles up to the postcode.
            /// </summary>
            [Fact]
            public async Task GivenAQuery15SpinnakerRiseSanctuaryLakesVictoriaAustralia_Geocode_ReturnsSomeData()
            {
                // Arrange.
                const string streetNumber = "15";
                const string street = "Spinnaker Rise";
                //const string suburb = "Sanctuary Lakes";
                const string state = "VIC";
                const string postcode = "3030";
                const string country = "AUSTRALIA";
                var query = $"{streetNumber} {street}, {state}, {country}";

                var componentFilters = new ComponentFilters
                {
                    PostalCode = postcode
                };
                var json = File.ReadAllText("Sample Data\\Google\\Result - 15 Spinnaker Rise Sanctuary Lakes Victoria.json");
                var response = FakeHttpMessageHandler.GetStringHttpResponseMessage(json);
                var options = new HttpMessageOptions
                {
                    RequestUri = new Uri("https://maps.googleapis.com/maps/api/geocode/json?address=15 Spinnaker Rise%2C VIC%2C AUSTRALIA&sensor=false&key=some api key&components=postal_code:3030"),
                    HttpResponseMessage = response
                };
                var httpClient = new HttpClient(new FakeHttpMessageHandler(options));
                var service = new GoogleMapsApiService("some api key", httpClient);

                // Act.
                var result = await service.GeocodeAsync(query, componentFilters);

                // Assert.
                result.ErrorMessage.ShouldBeNullOrEmpty();
                result.Results.Count.ShouldBe(1);

                var data = result.Results.First();
                // NOTE: Yes! The query is for a location in Sanctuary Point (even though that exact suburb name isn't provided)
                //       and the result is for Point Cook.
                data.FormattedAddress.ShouldBe("15 Spinnaker Rise, Point Cook VIC 3030, Australia");
                data.Geometry.Location.Lat.ShouldBe(-37.899923);
                data.Geometry.Location.Lng.ShouldBe(144.775248);

                options.NumberOfTimesCalled.ShouldBe(1);
            }
        }
    }
}
