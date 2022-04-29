using Microsoft.Extensions.DependencyInjection;
using StartOfNewPath.Identity.Interfaces;
using StartOfNewPath.Identity.Services;

namespace StartOfNewPath.Identity.Configuration
{
    public static class ServiceCollectionExtensions
    {
        public static void RegisterDependenciesI(this IServiceCollection services)
        {
            services.AddScoped<ITokenService, TokenService>();
        }
    }
}