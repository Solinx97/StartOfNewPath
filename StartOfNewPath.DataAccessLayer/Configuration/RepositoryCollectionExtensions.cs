using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StartOfNewPath.DataAccessLayer.Data;
using StartOfNewPath.DataAccessLayer.Entities;
using StartOfNewPath.DataAccessLayer.Interfaces;
using StartOfNewPath.DataAccessLayer.Repositories;

namespace StartOfNewPath.DataAccessLayer.Configuration
{
    public static class RepositoryCollectionExtensions
    {
        public static void RegisterDependenciesDAL(this IServiceCollection services, IConfiguration configuration, string connectionName)
        {
            string connection = configuration.GetConnectionString(connectionName);
            services.AddDbContext<SONPContext>(options => options.UseSqlServer(connection));

            services.AddScoped<IGenericRepository<Course>, GenericRepository<Course>>();
            services.AddScoped<ITokenRepository, TokenRepository>();
        }
    }
}