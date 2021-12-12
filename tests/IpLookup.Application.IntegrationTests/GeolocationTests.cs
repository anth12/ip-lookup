using IpLookup.ApiHost;
using Microsoft.AspNetCore.Mvc.Testing;
using NUnit.Framework;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Json;
using IpLookup.Domain;

namespace IpLookup.Application.IntegrationTests
{
    public class GeolocationTests
    {
        private HttpClient _httpClient;

        [SetUp]
        public void Setup()
        {
            var application = new WebApplicationFactory<Program>();

            _httpClient = application.CreateClient();
        }

        [TestCase("")]
        [TestCase("abc")]
        [TestCase("127.0.0.1")]
        public async Task Get_GivenInvalidIp_ThenBadRequest(string scenario)
        {
            // Act
            var response = await _httpClient.GetAsync($"/Geolocation?ip={scenario}");

            // Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);            
        }

        [TestCase("52.30.119.164")]
        public async Task Get_GivenValidIp_ThenOk(string scenario)
        {
            // Act
            var response = await _httpClient.GetAsync($"/Geolocation?ip={scenario}");
            var payload = await response.Content.ReadFromJsonAsync<Location>();

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            Assert.IsNotEmpty(payload.City);
            Assert.IsNotEmpty(payload.Country);
            Assert.IsNotEmpty(payload.Continent);
        }
    }
}