using System;

using MetaWeather.Api.Data;
using MetaWeather.Api.Models;
using MetaWeather.Application;
using MetaWeather.Core.Interfaces;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

using Refit;

namespace MetaWeather.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration) => Configuration = configuration;

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "MetaWeather Api V1");
            });

            if(env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            } else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseIdentityServer();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(name: "default", pattern: "{controller}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";

                if(env.IsDevelopment())
                {
                    spa.UseReactDevelopmentServer(npmScript: "start");
                }
            });
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // An alternative to IOptions, that enables access within the ConfigureService method
            var apiOptions = new ApiOptions();
            Configuration.GetSection("ApiOptions").Bind(apiOptions);

            services.AddSingleton(apiOptions);

            //services.AddAuthentication("Bearer")
            //    .AddJwtBearer("Bearer",
            //                  options =>
            //    {
            //        options.Authority = apiOptions.Authority;
            //        options.RequireHttpsMetadata = true;

            //        options.Audience = apiOptions.Audience;
            //    });


            services.AddRefitClient<IMetaWeatherService>()
                .ConfigureHttpClient(c =>
                {
                    c.BaseAddress = new Uri(apiOptions.ApiUrl);
                });

            services.AddScoped<IApiProxy, ApiProxy>();

            //services.AddControllers();

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlite(Configuration.GetConnectionString("DefaultConnection")));

            services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddIdentityServer().AddApiAuthorization<ApplicationUser, ApplicationDbContext>();

            services.AddAuthentication().AddIdentityServerJwt();

            services.AddControllersWithViews();
            services.AddRazorPages();

            // In production, the React files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/build";
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1",
                             new OpenApiInfo
                    {
                        Version = "v1",
                        Title = "MetaWeather Api",
                        Description = "Sample Api for Assessment task",
                        //TermsOfService = new Uri("https://example.com/terms"),
                        Contact =
                    new OpenApiContact
                            {
                                Name = "David McQuiggin",
                                Email = string.Empty,
                                Url = new Uri("https://twitter.com/mcquiggd"),
                            },
                        License =
                    new OpenApiLicense
                            { Name = "Use under LICX", Url = new Uri("https://example.com/license"), }
                    });
            });
        }

        public IConfiguration Configuration { get; }
    }
}