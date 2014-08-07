using System.Threading.Tasks;

namespace WorldDomination.Spatial.Services.ApiServices.GoogleMaps
{
    public interface IGoogleMapsApiService
    {
        Task<GoogleMapsResponse> GeocodeAsync(string query, ComponentFilters filters);
    }
}