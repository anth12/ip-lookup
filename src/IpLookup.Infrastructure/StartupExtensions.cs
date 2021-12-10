using Azure.Data.Tables;
using IpLookup.Infrastructure.Api;
using IpLookup.Infrastructure.Persistence;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;

namespace IpLookup.Infrastructure
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton(s => new TableClient(configuration.GetConnectionString("CosmosTableApi"), "Locations"));

            services.AddSingleton<IGeoLocationRepository, GeoLocationRepository>();

            services.AddSingleton<IGeoLocationApi, GeoLocationApi>(s => new GeoLocationApi(
                endpoint: configuration["IpGeoLocation:Endpoint"],
                apiKey: configuration["IpGeoLocation:ApiKey"],
                httpClient: new HttpClient()
            ));

            return services;
        }

    }
}
