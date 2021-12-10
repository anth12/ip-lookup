using IpLookup.Domain;
using IpLookup.Infrastructure.Api;
using IpLookup.Infrastructure.Persistence;
using System.Threading.Tasks;

namespace IpLookup.Application
{
    internal class GeoLocationService : IGeoLocationService
    {
        private readonly IGeoLocationRepository _geoLocationRepository;
        private readonly IGeoLocationApi _geoLocationApi;
        
        public GeoLocationService(IGeoLocationRepository geoLocationRepository, IGeoLocationApi geoLocationApi)
        {
            _geoLocationRepository = geoLocationRepository;
            _geoLocationApi = geoLocationApi;
        }

        public async Task<Location> GetLocationFromIpAddress(string ipAddress)
        {
            var location = await _geoLocationRepository.GetAsync(ipAddress);

            if(location != null)
                return location;

            location = await _geoLocationApi.GetByIpAddress(ipAddress);

            await _geoLocationRepository.AddAsync(ipAddress, location);

            return location;
        }
    }
}
