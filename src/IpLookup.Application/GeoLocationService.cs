using IpLookup.Domain;
using IpLookup.Infrastructure.TableStorage;
using System.Threading.Tasks;

namespace IpLookup.Application
{
    internal class GeoLocationService : IGeoLocationService
    {
        private readonly IGeoLocationRepository _geoLocationRepository;

        public GeoLocationService(IGeoLocationRepository geoLocationRepository)
        {
            _geoLocationRepository = geoLocationRepository;
        }

        public async Task<Location> GetLocationFromIpAddress(string ipAddress)
        {
            var location = await _geoLocationRepository.GetLocationAsync(ipAddress);

            if(location != null)
                return location;

            // TODO fetch from API
            location = new Location("test", "", "", new GeoCoordinate(1, 2), 0);

            await _geoLocationRepository.AddLocationAsync(ipAddress, location);

            return location;
        }
    }
}
