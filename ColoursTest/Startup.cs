using System;
using ColoursTest.AppServices.Interfaces;
using ColoursTest.AppServices.Services;
using ColoursTest.Infrastructure.Factories;
using ColoursTest.Domain.Interfaces;
using ColoursTest.Domain.Models;
using ColoursTest.Infrastructure.Interfaces;
using ColoursTest.Infrastructure.Middleware;
using ColoursTest.Infrastructure.Repositories;
using ColoursTest.Web.Common;
using ColoursTest.Web.Filters;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Cors.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using NLog.Extensions.Logging;
using NLog.Web;

namespace ColoursTest.Web
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", false, true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public static IConfigurationRoot Configuration;

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddCors(options =>
            {
                options.AddPolicy("AllowCORS", builder =>
                {
                    builder.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
                    //builder.WithOrigins("http://localhost:2112").WithMethods("GET, POST, PUT, OPTIONS").AllowAnyHeader();
                });
            });

            services.AddMvc(options =>
            {
                options.Filters.Add(typeof(CustomExceptionFilterAttribute));
                var policy = new AuthorizationPolicyBuilder()
                                .RequireAuthenticatedUser()
                                .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                                .Build();

                options.Filters.Add(new AuthorizeFilter(policy));

                options.Filters.Add(new CorsAuthorizationFilterFactory("AllowCORS"));
            });

            services.AddScoped<CustomExceptionFilterAttribute>();
            
            services.AddSingleton<IConfiguration>(Configuration);
            services.AddSingleton<ILoggerFactory, LoggerFactory>();
            services.AddSingleton<IDbConnectionFactory, SqlConnectionFactory>();
            services.AddSingleton<IMongoConnectionFactory, MongoConnectionFactory>();

            services.AddTransient<IPersonRepository, PersonRepository>();
            services.AddTransient<IColourRepository, ColourRepository>();
            services.AddTransient<IPersonService, PersonService>();
            services.AddTransient<IColourService, ColourService>();

            services.AddAuthentication(options => options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        options.RequireHttpsMetadata = false;
                        options.SaveToken = true;
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            RequireExpirationTime = true,
                            RequireSignedTokens = true,
                            ValidateIssuerSigningKey = true,
                            IssuerSigningKey = TokenAuthOption.Key,
                            ValidateAudience = true,
                            ValidAudience = TokenAuthOption.Audience,
                            ValidateIssuer = true,
                            ValidIssuer = TokenAuthOption.Issuer,
                            ValidateLifetime = true,
                            ClockSkew = TimeSpan.Zero
                        };
                    });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddNLog();
            app.AddNLogWeb();

            app.UseMiddleware<ExceptionMiddleware>();
            app.UseAuthentication();

            app.UseMvc();
        }
    }
}