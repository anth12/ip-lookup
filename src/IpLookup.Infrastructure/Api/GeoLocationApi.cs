using IpLookup.Domain;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Json;

namespace IpLookup.Infrastructure.Api
{
    internal class GeoLocationApi : IGeoLocationApi, IDisposable
    {
        public readonly string _endpoint;
        public readonly string _apiKey;
        private readonly HttpClient _httpClient;

        public GeoLocationApi(string endpoint, string apiKey, HttpClient httpClient)
        {
            _endpoint = endpoint;
            _apiKey = apiKey;
            _httpClient = httpClient;
        }

        public async Task<Location> GetByIpAddress(string ipAddress)
        {
            var requestUri = $"{_endpoint}?ip={ipAddress}&apiKey={_apiKey}";

            var response = await _httpClient.GetFromJsonAsync<Models.Location>(requestUri);

            return new Location(
                city: response.City,
                country: response.CountryName,
                continent: response.ContinentName,
                coordinate: new GeoCoordinate(response.Latitude, response.Longitude),
                timezoneOffset: response.TimeZone.Offset
            );
        }

        public void Dispose()
        {
            _httpClient?.Dispose();
        }
    }
}
