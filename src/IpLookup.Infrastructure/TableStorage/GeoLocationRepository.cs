using Azure;
using Azure.Data.Tables;
using IpLookup.Domain;
using IpLookup.Infrastructure.TableStorage.Entities;
using Microsoft.Extensions.Caching.Memory;
using System.Threading.Tasks;

namespace IpLookup.Infrastructure.TableStorage
{
    internal class GeoLocationRepository : IGeoLocationRepository
    {
        private readonly TableClient _tableClient;
        private readonly IMemoryCache _memoryCache;
        
        public GeoLocationRepository(TableClient tableClient, IMemoryCache memoryCache)
        {
            _tableClient = tableClient;
            _memoryCache = memoryCache;
        }

        public async Task AddLocationAsync(string ipAddress, Location location)
        {
            await _tableClient.AddEntityAsync(new GeoLocationEntity
            {
                PartitionKey = ipAddress,
                RowKey = ipAddress,
                City = location.City,
                Country = location.Country,
                Continent = location.Continent,
                Latitude = location.Coordinate.Latitude,
                Longitude = location.Coordinate.Longitude,
                TimezoneOffset = location.TimezoneOffset
            });

            var cacheKey = BuildMemoryCacheKey(ipAddress);
            _memoryCache.Set(cacheKey, location);
        }

        public async Task<Location> GetLocationAsync(string ipAddress)
        {
            var cacheKey = BuildMemoryCacheKey(ipAddress);

            if (_memoryCache.TryGetValue(cacheKey, out Location location))
                return location;

            Response<GeoLocationEntity> entity;
            try 
            { 
                entity = await _tableClient.GetEntityAsync<GeoLocationEntity>(ipAddress, ipAddress);
            }
            catch(RequestFailedException ex) when (ex.Status == 404)
            {
                return null;
            }

            if (entity.Value == null)
                return null;

            location = MapGeoLocation(entity);

            _memoryCache.Set(cacheKey, location);

            return location;
        }

        private static Location MapGeoLocation(Azure.Response<GeoLocationEntity> entity)
        {
            return new Location(
                city: entity.Value.City,
                country: entity.Value.Country,
                continent: entity.Value.Continent,
                coordinate: new GeoCoordinate(entity.Value.Latitude, entity.Value.Longitude),
                timezoneOffset: entity.Value.TimezoneOffset
            );
        }

        private string BuildMemoryCacheKey(string ipAddress)
            => $"GeoLocation:{ipAddress}";

    }
}
