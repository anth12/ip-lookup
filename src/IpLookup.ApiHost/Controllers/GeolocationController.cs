﻿using IpLookup.Application;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using System.Net;
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
        public async Task<IActionResult> Get(string ip)
        {
            if (!IPAddress.TryParse(ip, out var ipAddress))
                return BadRequest($"'{ip}' is not a valid IP address.");
            try
            {
                var result = await _geoLocationService.GetLocationFromIpAddressAsync(ipAddress.ToString());
                
                return Ok(result);
            }
            catch(ValidationException validation)
            {
                return BadRequest(validation.Message);
            }
        }
    }
}
