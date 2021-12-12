using IpLookup.Domain;
using IpLookup.Infrastructure.Api;
using IpLookup.Infrastructure.Persistence;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace IpLookup.Application
{
    internal class GeoLocationService : IGeoLocationService
    {
        private readonly ILogger<GeoLocationService> _logger;
        private readonly IGeoLocationRepository _geoLocationRepository;
        private readonly IGeoLocationApi _geoLocationApi;
        
        public GeoLocationService(ILogger<GeoLocationService> logger, IGeoLocationRepository geoLocationRepository, IGeoLocationApi geoLocationApi)
        {
            _logger = logger;
            _geoLocationRepository = geoLocationRepository;
            _geoLocationApi = geoLocationApi;
        }

        public async Task<Location> GetLocationFromIpAddressAsync(string ipAddress)
        {
            var location = await _geoLocationRepository.GetAsync(ipAddress);

            if(location != null)
                return location;

            try
            {
                location = await _geoLocationApi.GetByIpAddressAsync(ipAddress);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Failed to fetch location for {ipAddress}", ipAddress);
                throw;
            }

            await _geoLocationRepository.AddAsync(ipAddress, location);

            return location;
        }
    }
}
