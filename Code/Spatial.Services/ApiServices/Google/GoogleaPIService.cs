using System.Linq;
using RestSharp;
using Shouldly;
using Spatial.Core.Services;

namespace Spatial.Services.ApiServices.Google
{
    public class GoogleApiService : IApiService
    {
        private const string BaseUrl = "http://maps.googleapis.com";

        public IRestClient RestClient { get; set; }

        public object Geocode(string query)
        {
            string.IsNullOrWhiteSpace(query).ShouldBe(false);

            var client = new RestClient(BaseUrl);
            var request = new RestRequest("maps/api/geocode/json", Method.GET);

            request.AddParameter("address", query);
            request.AddParameter("sensor", "false");

            IRestResponse<GoogleResponse> response = client.Execute<GoogleResponse>(request);

            return response.Data != null &&
                   response.Data.Results != null &&
                   response.Data.Results.Any()
                ? response.Data
                : null;
        }
    }
}