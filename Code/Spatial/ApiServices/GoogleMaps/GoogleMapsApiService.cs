using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WorldDomination.Net.Http;

namespace WorldDomination.Spatial.ApiServices.GoogleMaps
{
    public class GoogleMapsApiService : IGoogleMapsApiService
    {
        public async Task<GoogleMapsResponse> GeocodeAsync(string query, ComponentFilters filters = null)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                throw new ArgumentNullException("query");
            }

            var requestUrl = new StringBuilder();
            requestUrl.AppendFormat("http://maps.googleapis.com/maps/api/geocode/json?address={0}&sensor=false",
                Uri.EscapeDataString(query));

            if (filters != null)
            {
                var components = ConvertCompenentFiltersToQuerystringParameter(filters);
                if (!string.IsNullOrWhiteSpace(components))
                {
                    requestUrl.AppendFormat("&components={0}", components);
                }
            }

            HttpResponseMessage response;
            using (var httpClient = HttpClientFactory.GetHttpClient())
            {
                response = await httpClient.GetAsync(requestUrl.ToString());
            }

            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                var errorMessage =
                    string.Format("Failed to retrieve a Google Maps GeoCode result. Status Code: {0}. Message: {1}",
                        response.StatusCode,
                        content);
                throw new Exception(errorMessage);
            }

            return JsonConvert.DeserializeObject<GoogleMapsResponse>(content);
        }

        // REF: https://developers.google.com/maps/documentation/geocoding/#ComponentFiltering
        private static string ConvertCompenentFiltersToQuerystringParameter(ComponentFilters filters)
        {
            if (filters == null)
            {
                throw new ArgumentNullException("filters");
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