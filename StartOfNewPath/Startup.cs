using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StartOfNewPath.BusinessLayer.Configuration;
using StartOfNewPath.BusinessLayer.Mapping;
using StartOfNewPath.Data;
using StartOfNewPath.DataAccessLayer.Entities.User;
using StartOfNewPath.Identity.Authentication;
using StartOfNewPath.Identity.Interfaces;
using StartOfNewPath.Identity.Services;
using StartOfNewPath.Identity.Settings;
using StartOfNewPath.Mapping;
using StartOfNewPath.Models.User;

namespace StartOfNewPath
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            RegisteringDependencies(services);

            string connection = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<SONPAuthContext>(option => option.UseSqlServer(connection));
            services.AddIdentity<ApplicationUserModel, ApplicationRole>()
                .AddEntityFrameworkStores<SONPAuthContext>();

            var settings = Configuration.GetSection(nameof(JWTSettings));
            var scheme = settings.GetValue<string>(nameof(JWTSettings.AuthScheme));
            services.Configure<JWTSettings>(settings);
            settings = Configuration.GetSection(nameof(UserApiSettings));
            services.Configure<UserApiSettings>(settings);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = scheme;
                options.DefaultScheme = scheme;
                options.DefaultChallengeScheme = scheme;
            })
            .AddScheme<AuthOptions, AuthHandler>(scheme, null);

            services.AddControllersWithViews();
            services.AddRazorPages();

            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/build";
            });

            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new ApiMappingProfile());
                mc.AddProfile(new MappingProfile());
            });

            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            //app.UseIdentityServer();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseReactDevelopmentServer(npmScript: "start");
                }
            });
        }

        private void RegisteringDependencies(IServiceCollection services)
        {
            services.RegisterDependenciesBL(Configuration, "DefaultConnection");

            services.AddSingleton<IJWTService, JWTService>();
        }
    }
}
