using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace WorldDomination.Spatial.ApiServices.GoogleMaps
{
    public class GoogleMapsApiService : IGoogleMapsApiService
    {
        private readonly string _apiKey;
        private readonly HttpClient _httpClient;


        public GoogleMapsApiService() : this(null, null)
        {
        }

        public GoogleMapsApiService(string apiKey) : this(apiKey, null)
        {
        }

        public GoogleMapsApiService(HttpClient httpClient = null) : this(null, httpClient)
        {
        }

        public GoogleMapsApiService(string apiKey, HttpClient httpClient)
        {
            if (!string.IsNullOrWhiteSpace(apiKey))
            {
                _apiKey = apiKey.Trim();
            }

            _httpClient = httpClient;
        }

        public async Task<GoogleMapsResponse> GeocodeAsync(string query, ComponentFilters filters = null)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                throw new ArgumentNullException(nameof(query));
            }

            var requestUrl = new StringBuilder();
            requestUrl.Append(
                $"https://maps.googleapis.com/maps/api/geocode/json?address={Uri.EscapeDataString(query)}&sensor=false");

            // Append the API key if it's been provided.
            if (!string.IsNullOrWhiteSpace(_apiKey))
            {
                requestUrl.Append($"&key={_apiKey}");
            }

            if (filters != null)
            {
                var components = ConvertCompenentFiltersToQuerystringParameter(filters);
                if (!string.IsNullOrWhiteSpace(components))
                {
                    requestUrl.Append($"&components={components}");
                }
            }

            var httpClient = _httpClient ?? new HttpClient();
            var response = await httpClient.GetAsync(requestUrl.ToString());
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                var errorMessage =
                    $"Failed to retrieve a Google Maps GeoCode result. Status Code: {response.StatusCode}. Message: {content}";
                throw new Exception(errorMessage);
            }

            return JsonConvert.DeserializeObject<GoogleMapsResponse>(content);
        }

        // REF: https://developers.google.com/maps/documentation/geocoding/#ComponentFiltering
        private static string ConvertCompenentFiltersToQuerystringParameter(ComponentFilters filters)
        {
            if (filters == null)
            {
                throw new ArgumentNullException(nameof(filters));
            }

            var items = new Dictionary<string, string>();

            if (!string.IsNullOrWhiteSpace(filters.Route))
            {
                items.Add("route", filters.Route);
            }

            if (!string.IsNullOrWhiteSpace(filters.Locality))
            {
                items.Add("locality", filters.Locality);
            }

            if (!string.IsNullOrWhiteSpace(filters.AdministrativeArea))
            {
                items.Add("administrative_area", filters.AdministrativeArea);
            }

            if (!string.IsNullOrWhiteSpace(filters.PostalCode))
            {
                items.Add("postal_code", filters.PostalCode);
            }

            if (!string.IsNullOrWhiteSpace(filters.CountryCodeIso))
            {
                items.Add("country", filters.CountryCodeIso);
            }

            if (!items.Any())
            {
                return string.Empty;
            }

            var queryString = new StringBuilder();
            foreach (var item in items)
            {
                if (queryString.Length > 0)
                {
                    queryString.Append("|");
                }

                queryString.AppendFormat("{0}:{1}", item.Key, item.Value);
            }

            return queryString.ToString();
        }
    }
}