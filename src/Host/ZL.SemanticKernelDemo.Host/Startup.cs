﻿using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using Microsoft.OpenApi.Models;


namespace ZL.SemanticKernelDemo.Host
{
    public static class Startup
    {

        // configure MediatR
        public static void ConfigureMediatR(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Startup).Assembly));
        }

        // configure CORS policy
        public static void ConfigureCors(this IServiceCollection services, string corsPolicy)
        {
            // cors policy
            services.AddCors(
                opt =>
                {
                    opt.AddPolicy(corsPolicy, builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
                });

        }

        // configure swagger
        public static void ConfigureSwagger(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                // c.EnableAnnotations();
                // c.SwaggerDoc("v1", new OpenApiInfo() { Title = "LaozaoShanghai Api 1.0", Version = "v1.0", Description = "API for LaoShanghai web app.", });

                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "RAG Demo API",
                    Description = "Semantic Kernel Demo",
                    Version = "v1.0",
                });

                // add bearer token support
                var securitySchema = new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                };

                c.AddSecurityDefinition("Bearer", securitySchema);
                var securityRequirement = new OpenApiSecurityRequirement
                {
                    { securitySchema, new[] { "Bearer" } }
                };

                c.AddSecurityRequirement(securityRequirement);
            });
        }

        // configure application insights
        public static void ConfigureApplicationInsights(this IServiceCollection services, IConfiguration configuration)
        {
            // retreive the connection string from key vault
            var appInsightsConnection = configuration[SecretKeys.AppInsightsConnection];
            var options = new ApplicationInsightsServiceOptions { ConnectionString = appInsightsConnection };
            services.AddApplicationInsightsTelemetry(options: options);
        }
    }
}
