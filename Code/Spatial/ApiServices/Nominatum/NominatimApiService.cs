using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WorldDomination.Net.Http;

namespace WorldDomination.Spatial.ApiServices.Nominatum
{
    public class NominatimApiService
    {
        // Reference: http://wiki.openstreetmap.org/wiki/Nominatim

        public string Email { get; set; }
        public int Limit { get; set; }

        public async Task<List<NominatimResponse>> GeocodeAsync(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                throw new ArgumentNullException("query");
            }

            var requestUrl = new StringBuilder();
            requestUrl.AppendFormat("http://nominatim.openstreetmap.org/search/{0}?format=json&limt={1}",
                query,
                Limit <= 0 ? 1 : Limit);

            var httpClient = HttpClientFactory.GetHttpClient();
            if (!string.IsNullOrWhiteSpace(Email))
            {
                httpClient.DefaultRequestHeaders.Add("user-agent", Email);
            }

            var requestUri = new Uri(requestUrl.ToString());
            var response = await httpClient.GetAsync(requestUri);
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                var errorMessage =
                    string.Format("Failed to retrieve a Nominatim GeoCode result. Status Code: {0}. Message: {1}",
                        response.StatusCode,
                        content);
                throw new Exception(errorMessage);
            }

            return JsonConvert.DeserializeObject<List<NominatimResponse>>(content);
        }
    }
}