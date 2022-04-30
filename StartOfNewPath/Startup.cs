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
using StartOfNewPath.Identity.Authentication;
using StartOfNewPath.Identity.Configuration;
using StartOfNewPath.Identity.Mapping;
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
            services.AddIdentity<ApplicationUserModel, ApplicationRoleModel>()
                .AddEntityFrameworkStores<SONPAuthContext>();

            var settings = Configuration.GetSection(nameof(TokenSettings));
            var scheme = settings.GetValue<string>(nameof(TokenSettings.AuthScheme));
            services.Configure<TokenSettings>(settings);
            settings = Configuration.GetSection(nameof(UserApiSettings));
            services.Configure<UserApiSettings>(settings);

            services.AddAuthentication(scheme).AddScheme<AuthOptions, AuthHandler>(scheme, null);

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
                mc.AddProfile(new IdentityMappingProfile());
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
            services.RegisterDependenciesI();
        }
    }
}
