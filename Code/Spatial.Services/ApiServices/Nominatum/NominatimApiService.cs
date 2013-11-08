using System.Collections.Generic;
using RestSharp;
using Shouldly;
using Spatial.Core.Services;

namespace Spatial.Services.ApiServices.Nominatum
{
    public class NominatimApiService : IApiService
    {
        // Reference: http://wiki.openstreetmap.org/wiki/Nominatim

        private const string BaseUrl = "http://nominatim.openstreetmap.org";
        public string Email { get; set; }
        public int Limit { get; set; }

        public IRestClient RestClient { get; set; }

        public object Geocode(string query)
        {
            string.IsNullOrWhiteSpace(query).ShouldBe(false);

            var client = new RestClient(BaseUrl);
            if (!string.IsNullOrWhiteSpace(Email))
            {
                client.UserAgent = Email;
            }

            var request = new RestRequest("search", Method.GET);
            request.AddParameter("format", "json");
            request.AddParameter("q", query);
            request.AddParameter("limit", Limit <= 0 ? 1 : Limit);

            var response = client.Execute<List<NominatimResponse>>(request);

            return response == null ||
                   response.Data == null ||
                   response.Data.Count <= 0
                ? null
                : response.Data;
        }
    }
}