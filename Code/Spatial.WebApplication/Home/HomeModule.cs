using System.Threading;
using System.Threading.Tasks;
using Nancy;
using Nancy.ModelBinding;
using WorldDomination.Spatial.ApiServices.GoogleMaps;

namespace Spatial.WebApplication.Home
{
    public class HomeModule : NancyModule
    {
        public HomeModule()
        {
            Get["/"] = GetHome;
            Get["/google", true] = PostGoogle;
        }

        private dynamic GetHome(dynamic nancy)
        {
            return View["home"];
        }

        private async Task<dynamic> PostGoogle(dynamic nancy,
            CancellationToken cancellationToken)
        {
            var command = this.Bind<GetGoogleCommand>();
            var service = new GoogleMapsApiService(command.GoogleApiKey);
            return await service.GeocodeAsync(command.Address);
        }
    }
}