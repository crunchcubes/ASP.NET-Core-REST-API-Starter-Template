using System.IO;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;
using Restful.Api.Helpers;
using Restful.Infrastructure.Database;
using Restful.Infrastructure.Extensions;
using Serilog;
using Serilog.Events;

namespace Restful.Api
{
    public class StartupIntegrationTest
    {
        public static IConfiguration Configuration { get; set; }
        private readonly ILoggerFactory _loggerFactory;

        public StartupIntegrationTest(IConfiguration configuration, ILoggerFactory loggerFactory)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.File(Path.Combine("logs", @"log.txt"), rollingInterval: RollingInterval.Day)
                .CreateLogger();

            _loggerFactory = loggerFactory;
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(options =>
                {
                    options.ReturnHttpNotAcceptable = true;
                })
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                })
                .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<MyContext>());

            services.AddMediaTypes();

            services.AddDbContext<MyContext>(options =>
            {
                options.UseInMemoryDatabase("RESTfulAPI");
                options.UseLoggerFactory(_loggerFactory);
            });

            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddScoped<IUrlHelper>(factory =>
            {
                var actionContext = factory.GetService<IActionContextAccessor>().ActionContext;
                return new UrlHelper(actionContext);
            });

            services.AddHttpCacheHeaders(
                expirationModelOptions =>
                {
                    expirationModelOptions.MaxAge = 300;
                },
                validationModelOptions =>
                {
                    validationModelOptions.AddMustRevalidate = true;
                });

            services.AddResponseCaching();

            services.AddMemoryCache();
            services.AddMyServices();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseMyExceptionHandler(_loggerFactory);
            // app.UseAuthentication();
            app.UseResponseCaching();
            app.UseHttpCacheHeaders();

            app.UseMvc();
        }
    }
}
