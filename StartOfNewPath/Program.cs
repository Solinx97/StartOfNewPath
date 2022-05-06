using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using StartOfNewPath.Identity.Security;
using StartOfNewPath.Initialization;
using StartOfNewPath.Models.User;
using System;
using System.Threading.Tasks;

namespace StartOfNewPath
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            JWTSecret.GenerateAccessSecretKey();
            JWTSecret.GenerateRefreshSecretKey();

            await StartInitialization(host);
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        public static async Task StartInitialization(IHost host)
        {
            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;
            try
            {
                var userManager = services.GetRequiredService<UserManager<ApplicationUserModel>>();
                var rolesManager = services.GetRequiredService<RoleManager<ApplicationRoleModel>>();
                await DbInitializer.InitializeAsync(userManager, rolesManager);
            }
            catch (Exception ex)
            {
                var logger = services.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "An error occurred while seeding the database.");
            }

            host.Run();
        }
    }
}
