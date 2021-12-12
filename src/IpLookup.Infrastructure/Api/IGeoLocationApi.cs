using System.Threading.Tasks;
using IpLookup.Domain;

namespace IpLookup.Infrastructure.Api
{
    public interface IGeoLocationApi
    {
        Task<Location> GetByIpAddressAsync(string ipAddress);
    }
}
