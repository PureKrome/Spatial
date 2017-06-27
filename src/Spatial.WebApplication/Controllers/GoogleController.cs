using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WorldDomination.Spatial.ApiServices.GoogleMaps;

// ReSharper disable ConsiderUsingConfigureAwait

namespace Spatial.WebApplication.Controllers
{
    [Route("google")]
    public class GoogleController
    {
        [HttpGet]
        [Route("geocode")]
        public async Task<IActionResult> GeocodeAsync(string address, string postcode, string key)
        {
            var service = new GoogleMapsApiService(key);
            ComponentFilters filters = null;
            if (!string.IsNullOrWhiteSpace(postcode))
            {
                filters = new ComponentFilters {PostalCode = postcode};
            }

            var response = await service.GeocodeAsync(address, filters);

            return new JsonResult(response);
        }
    }
}