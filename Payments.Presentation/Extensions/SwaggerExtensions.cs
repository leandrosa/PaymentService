using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Payments.Presentation.Extensions
{
    public static class SwaggerExtensions
    {
        public static SwaggerGenOptions ConfigureSecurityDefinition(this SwaggerGenOptions options)
        {
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "Type into the textbox: Bearer {your JWT token}.",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
            });

            var security =
                new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Id = "Bearer",
                                Type = ReferenceType.SecurityScheme
                            },
                            UnresolvedReference = true
                        },
                        new string[] {}
                    }
                };

            options.AddSecurityRequirement(security);

            return options;
        }
    }
}
