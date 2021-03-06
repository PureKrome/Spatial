using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace WorldDomination.Spatial.ApiServices.Nominatum
{
    // Reference: http://wiki.openstreetmap.org/wiki/Nominatim
    public class NominatimApiService
    {
        private readonly HttpClient _httpClient;

        public NominatimApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

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

            var httpRequestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(requestUrl.ToString())
            };

            if (!string.IsNullOrWhiteSpace(Email))
            {
                httpRequestMessage.Headers.Add("user-agent", Email);
            }

            var response = await _httpClient.SendAsync(httpRequestMessage);
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
