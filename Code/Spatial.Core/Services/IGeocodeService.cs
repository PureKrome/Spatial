using Spatial.Core.Models;

namespace Spatial.Core.Services
{
    public interface IGeocodeService
    {
        Coordinate Geocode(string query);
    }
}