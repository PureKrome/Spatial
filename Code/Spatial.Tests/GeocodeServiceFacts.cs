using FakeItEasy;
using Spatial.Services;
using Xunit;

namespace Spatial.Tests
{
    public class GeocodeServiceFacts
    {
        public class GeocodeFacts
        {
            [Fact]
            public void GivenAValidQueryAndNominatiumKnowsThisAddress_Geocode_ReturnsACoordinateFromNominatum()
            {
                // Arrange.
                var geocodeService = new GeocodeService();
                const string query = "blah blah blah";

                // Act.
                var coordinate = geocodeService.Geocode(query);
            }
        }
    }
}