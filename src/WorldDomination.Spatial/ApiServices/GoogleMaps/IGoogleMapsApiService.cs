using System.Threading.Tasks;

namespace WorldDomination.Spatial.ApiServices.GoogleMaps
{
    public interface IGoogleMapsApiService
    {
        Task<GoogleMapsResponse> GeocodeAsync(string query, ComponentFilters filters = null);
    }
}