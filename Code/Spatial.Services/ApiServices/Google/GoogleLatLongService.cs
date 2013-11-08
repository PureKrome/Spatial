using System;
using System.Linq;
using CuttingEdge.Conditions;
using RestSharp;

namespace AppianMedia.XWing.Services.LatLongShit.Google
{
    public class GoogleLatLongService : ILatLongService
    {
        private const string BaseUrl = "http://maps.googleapis.com";

        public IRestClient RestClient { get; set; }

        public LatLong Geocode(SearchAddress address)
        {
            Condition.Requires(address).IsNotNull();

            var client = new RestClient(BaseUrl);
            var request = new RestRequest("maps/api/geocode/json", Method.GET);

            request.AddParameter("address", address.ToAddress);
            request.AddParameter("sensor", "false");

            var response = client.Execute<GoogleResponse>(request);

            return response.Data != null &&
                   response.Data.results != null &&
                   response.Data.results.Any() &&
                   response.Data.results.First() != null &&
                   response.Data.results.First().geometry != null &&
                   response.Data.results.First().geometry.location != null
                       ? new LatLong
                         {
                             Latitude = Convert.ToDecimal(response.Data.results.First().geometry.location.lat),
                             Longitude = Convert.ToDecimal(response.Data.results.First().geometry.location.lng)
                         }
                       : null;
        }
    }
}