using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using RestSharp;
using Shouldly;
using Spatial.Core.Services;

namespace Spatial.Services.ApiServices.GoogleMaps
{
    public class GoogleMapsApiService : IApiService
    {
        private const string BaseUrl = "http://maps.googleapis.com";

        public IRestClient RestClient { get; set; }

        public object Geocode(string query)
        {
            return Geocode(query, null);
        }

        public object Geocode(string query, ComponentFilters filters)
        {
            string.IsNullOrWhiteSpace(query).ShouldBe(false);

            var client = new RestClient(BaseUrl);
            var request = new RestRequest("maps/api/geocode/json", Method.GET);

            request.AddParameter("address", query);
            request.AddParameter("sensor", "false");

            if (filters != null)
            {
                var components = ConvertCompenentFiltersToQuerystringParameter(filters);
                if (!string.IsNullOrWhiteSpace(components))
                {
                    request.AddParameter("components", components);
                }
            }

            IRestResponse<GoogleMapsResponse> response = client.Execute<GoogleMapsResponse>(request);

            return response.Data != null &&
                   response.Data.Results != null &&
                   response.Data.Results.Any()
                ? response.Data
                : null;
        }

        // REF: https://developers.google.com/maps/documentation/geocoding/#ComponentFiltering
        private static string ConvertCompenentFiltersToQuerystringParameter(ComponentFilters filters)
        {
            filters.ShouldNotBe(null);

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