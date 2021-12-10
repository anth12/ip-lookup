using Azure.Data.Tables;
using IpLookup.Infrastructure.TableStorage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IpLookup.Infrastructure
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("CosmosTableApi");
            services.AddSingleton(s => new TableClient(connectionString, "Locations"));

            services.AddSingleton<IGeoLocationRepository, GeoLocationRepository>();

            return services;
        }

    }
}
