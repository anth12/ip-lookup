using IpLookup.Domain;
using IpLookup.Infrastructure.Api;
using IpLookup.Infrastructure.Persistence;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Net;
using System.Threading.Tasks;

namespace IpLookup.Application.UnitTests
{
    public class GeoLocationServiceTests
    {
        private Mock<ILogger<GeoLocationService>> _loggerMock;
        private Mock<IGeoLocationRepository> _repositoryMock;
        private Mock<IGeoLocationApi> _apiMock;
        private IGeoLocationService _geoLocationService;

        private const string IpAddressMock = "192.16.155.0";
        private readonly Location _locationMock = new ("mock-city", "mock-country", "mock-continent", new(1, 2), 3);

        [SetUp]
        public void SetUp()
        {
            _loggerMock = new Mock<ILogger<GeoLocationService>>(MockBehavior.Strict);
            _repositoryMock = new Mock<IGeoLocationRepository>(MockBehavior.Strict);
            _apiMock = new Mock<IGeoLocationApi>(MockBehavior.Strict);

            _geoLocationService = new GeoLocationService(_loggerMock.Object, _repositoryMock.Object, _apiMock.Object);
        }

        [Test]
        public async Task GetLocationFromIpAddressAsync_GivenPersistedValue_ThenReturns()
        {
            // Arrange
            _repositoryMock.Setup(r => r.GetAsync(It.Is<string>(s => s == IpAddressMock)))
                .ReturnsAsync(_locationMock);

            // Act
            var result = await _geoLocationService.GetLocationFromIpAddressAsync(IpAddressMock);

            // Assert
            Assert.AreEqual(_locationMock, result);
        }

        [Test]
        public void GetLocationFromIpAddressAsync_GivenApiError_ThenLogAndThrow()
        {
            // Arrange
            _repositoryMock.Setup(r => r.GetAsync(It.Is<string>(s => s == IpAddressMock)))
                .ReturnsAsync((Location)null);

            _apiMock.Setup(a => a.GetByIpAddressAsync(It.Is<string>(s => s == IpAddressMock)))
                .Throws(new ApiException(HttpStatusCode.InternalServerError, "mock-response"));

            _loggerMock.Setup(l => l.Log(
                    It.Is<LogLevel>(l => l == LogLevel.Error),
                    It.IsAny<EventId>(),
                    It.IsAny<It.IsAnyType>(),
                    It.IsAny<ApiException>(),
                    It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true))
            ).Verifiable();

            // Act
            var exception = Assert.ThrowsAsync<ApiException>(() =>
                _geoLocationService.GetLocationFromIpAddressAsync(IpAddressMock));

            // Assert
            Assert.AreEqual(HttpStatusCode.InternalServerError, exception.StatusCode);

            _loggerMock.VerifyAll();
        }

        [Test]
        public async Task GetLocationFromIpAddressAsync_GivenFetchedFromApi_ThenPersist()
        {
            // Arrange
            _repositoryMock.Setup(r => r.GetAsync(It.Is<string>(s => s == IpAddressMock)))
                .ReturnsAsync((Location)null);

            _apiMock.Setup(a => a.GetByIpAddressAsync(It.Is<string>(s => s == IpAddressMock)))
                .ReturnsAsync(_locationMock);

            _repositoryMock.Setup(r => r.AddAsync(
                It.Is<string>(s => s == IpAddressMock),
                It.Is<Location>(l => l.City == _locationMock.City
                                    && l.Country == _locationMock.Country)))
                .Returns(Task.CompletedTask)
                .Verifiable();

            // Act
            var result = await _geoLocationService.GetLocationFromIpAddressAsync(IpAddressMock);

            // Assert
            Assert.AreEqual(_locationMock, result);

            _repositoryMock.VerifyAll();
        }
    }
}
