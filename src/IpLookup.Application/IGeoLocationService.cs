using IpLookup.Domain;
using System.Threading.Tasks;

namespace IpLookup.Application
{
    public interface IGeoLocationService
    {
        Task<Location> GetLocationFromIpAddressAsync(string ipAddress);
    }
}
