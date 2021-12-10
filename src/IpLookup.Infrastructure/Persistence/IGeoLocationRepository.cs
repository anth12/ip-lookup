using IpLookup.Domain;
using System.Threading.Tasks;

namespace IpLookup.Infrastructure.Persistence
{
    public interface IGeoLocationRepository
    {
        Task AddAsync(string ipAddress, Location location);
        Task<Location> GetAsync(string ipAddress);
    }
}
