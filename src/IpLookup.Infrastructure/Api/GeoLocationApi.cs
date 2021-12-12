using IpLookup.Domain;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Json;
using System.ComponentModel.DataAnnotations;
using System.Net;

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

        public async Task<Location> GetByIpAddressAsync(string ipAddress)
        {
            var requestUri = $"{_endpoint}?ip={ipAddress}&apiKey={_apiKey}";

            var response = await _httpClient.GetAsync(requestUri);

            if(response.StatusCode == HttpStatusCode.OK)
            {
                var result = await HandleSuccessResponse(response);
                return result;
            }

            if (response.StatusCode == HttpStatusCode.Locked)
            {
                var validation = await response.Content.ReadFromJsonAsync<Models.ValidationResponse>();
                throw new ValidationException(validation.Message);
            }
            
            var responseMessage = await response.Content.ReadAsStringAsync();
            throw new ApiException(response.StatusCode, responseMessage);
        }

        private static async Task<Location> HandleSuccessResponse(HttpResponseMessage response)
        {
            var payload = await response.Content.ReadFromJsonAsync<Models.LocationResponse>();

            return new Location(
                city: payload.City,
                country: payload.CountryName,
                continent: payload.ContinentName,
                coordinate: new GeoCoordinate(payload.Latitude, payload.Longitude),
                timezoneOffset: payload.TimeZone.Offset
            );
        }

        public void Dispose()
        {
            _httpClient?.Dispose();
        }
    }
}
