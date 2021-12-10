using IpLookup.Domain;
using System.Threading.Tasks;

namespace IpLookup.Infrastructure.TableStorage
{
    public interface IGeoLocationRepository
    {
        Task AddLocationAsync(string ipAddress, Location location);
        Task<Location> GetLocationAsync(string ipAddress);
    }
}
