using IpLookup.Application;
using IpLookup.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace IpLookup.ApiHost.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GeolocationController : ControllerBase
    {
        private readonly ILogger<GeolocationController> _logger;
        private readonly IGeoLocationService _geoLocationService;

        public GeolocationController(ILogger<GeolocationController> logger, IGeoLocationService geoLocationService)
        {
            _logger = logger;
            _geoLocationService = geoLocationService;
        }

        [HttpGet]
        public Task<Location> Get(string ipAddress)
        {
            return _geoLocationService.GetLocationFromIpAddress(ipAddress);
        }
    }
}
