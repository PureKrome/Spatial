namespace Spatial.Core.Services
{
    public interface IApiService
    {
        object Geocode(string query);
    }
}