using System;
using System.Net.Http;
using System.Text.Json.Serialization;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Payments.Application;
using Payments.Infrastructure;
using Payments.Infrastructure.Identity;
using Payments.Infrastructure.Persistence;
using Payments.Presentation.Authentication;
using Payments.Presentation.Extensions;
using Payments.Presentation.Filters;
using Payments.Presentation.Middlewares;

namespace Payments.Presentation
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(Configuration.GetSection("jwtTokenConfiguration").Get<JwtTokenConfiguration>());
            services.AddScoped<JwtTokenManager>();

            services.AddControllers();
           
            services.AddInfrastructure(Configuration);
            services.AddApplication(Configuration);

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddHttpContextAccessor();

            services.AddHealthChecks()
                .AddDbContextCheck<ApplicationDbContext>("Infrastructure - SQL Server", HealthStatus.Unhealthy)
                .AddUrlGroup(new Uri(Configuration.GetValue<string>("IssuerConfiguration:Uri")), "Issuer Dependency", HealthStatus.Unhealthy, timeout: TimeSpan.FromSeconds(1))
                .AddUrlGroup(new Uri(Configuration.GetValue<string>("ElasticConfiguration:Uri")), "Elasticsearch", HealthStatus.Unhealthy, timeout: TimeSpan.FromSeconds(1))
                .AddApplicationInsightsPublisher();

            services.AddHealthChecksUI(setup =>
            {
                setup.UseApiEndpointHttpMessageHandler(sp =>
                {
                    return new HttpClientHandler
                    {
                        ClientCertificateOptions = ClientCertificateOption.Manual,
                        ServerCertificateCustomValidationCallback = (httpRequestMessage, cert, cetChain, policyErrors) => { return true; }
                    };
                });
            }).AddInMemoryStorage();

            services.AddSwaggerGen(c =>
            {
                c.ConfigureSecurityDefinition();
            });

            services.SetupAuthentication(Configuration);

            services.AddMvc(options =>
            {
                options.UseCentralRoutePrefix(new RouteAttribute("payment/v1"));

                options.Filters.Add(new ApiExceptionFilterAttribute());
            })
            .AddJsonOptions(opts =>
            {
                opts.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMiddleware<RequestLoggingMiddleware>();

            app.UseHealthChecks("/healthchecks", new HealthCheckOptions
            {
                Predicate = _ => true,
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });

            app.UseHealthChecksUI(options =>
            {
                options.UIPath = "/healthchecks-ui";
                options.ApiPath = "/healthchecks-api";
            });

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Checkout Payment Gateway API V1");
            });

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action}/");

                endpoints.MapHealthChecksUI();
            });
        }
    }
}
