using Microsoft.Extensions.DependencyInjection;

namespace IpLookup.Application
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddSingleton<IGeoLocationService, GeoLocationService>();

            return services;
        }

    }
}
