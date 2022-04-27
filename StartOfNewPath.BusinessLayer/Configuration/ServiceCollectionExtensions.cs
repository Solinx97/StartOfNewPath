using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StartOfNewPath.BusinessLayer.DTO;
using StartOfNewPath.BusinessLayer.Interfaces;
using StartOfNewPath.BusinessLayer.Services;
using StartOfNewPath.DataAccessLayer.Configuration;

namespace StartOfNewPath.BusinessLayer.Configuration
{
    public static class ServiceCollectionExtensions
    {
        public static void RegisterDependenciesBL(this IServiceCollection services, IConfiguration configuration, string connectionName)
        {
            services.RegisterDependenciesDAL(configuration, connectionName);

            services.AddScoped<IService<CourseDto>, CourseService>();
        }
    }
}
