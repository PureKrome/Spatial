using System.Threading.Tasks;

namespace Spatial.Services.ApiServices.GoogleMaps
{
    public interface IGoogleMapsApiService
    {
        Task<GoogleMapsResponse> GeocodeAsync(string query, ComponentFilters filters);
    }
}