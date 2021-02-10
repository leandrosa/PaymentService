using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;
using Payments.Application.Common.Configurations;
using Payments.Application.Interfaces;
using Payments.Infrastructure.Gateways;
using Payments.Infrastructure.Gateways.Mock;
using Payments.Infrastructure.Identity;
using Payments.Infrastructure.Notifications;
using Payments.Infrastructure.Persistence;
using Payments.Presentation.Services;
using Polly;
using Polly.Extensions.Http;
using System;
using System.Net.Http;

namespace Payments.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            if (configuration.GetValue<bool>("UseInMemoryDatabase"))
            {
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseInMemoryDatabase("CheckoutDb"));
            }
            else
            {
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlServer(
                        configuration.GetConnectionString("DefaultConnection"),
                        b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));
            }

            services.AddScoped<IApplicationDbContext>(provider => provider.GetService<ApplicationDbContext>());
            services.AddScoped<IIdentityService, IdentityService>();
            services.AddScoped<INotificationContext, NotificationContext>();

            services.AddIdentityCore<ApplicationUser>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            var issuerConfiguration = configuration.GetSection("IssuerConfiguration").Get<IssuerConfiguration>();

            if (issuerConfiguration.Mocked)
            {
                services.AddScoped<IIssuerApiClient, IssuerApiClientMock>();
            }
            else
            {
                services.AddHttpClient<IIssuerApiClient, IssuerApiClient>(client =>
                {
                    client.BaseAddress = new Uri(issuerConfiguration.Uri);
                    client.DefaultRequestHeaders.Add(HeaderNames.UserAgent, issuerConfiguration.UserAgent);
                    client.Timeout = TimeSpan.FromSeconds(issuerConfiguration.TimeoutInSeconds);
                })
                .ConfigureHttpMessageHandlerBuilder((c) =>
                    new HttpClientHandler()
                    {
                        AutomaticDecompression = System.Net.DecompressionMethods.GZip
                    }
                )
                .AddPolicyHandler(GetRetryPolicy());

                services.AddScoped<IIssuerApiClient, IssuerApiClient>();
            }

            return services;
        }

        static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(1, retryAttempt)));
        }
    }
}
