using System;
using ColoursTest.AppServices.Interfaces;
using ColoursTest.AppServices.Services;
using ColoursTest.Infrastructure.Factories;
using ColoursTest.Domain.Interfaces;
using ColoursTest.Infrastructure.Interfaces;
using ColoursTest.Infrastructure.Repositories;
using ColoursTest.Web.Common;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
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
            services.AddMvc();

            services.AddSingleton<IConfiguration>(Configuration);
            services.AddSingleton<ILoggerFactory, LoggerFactory>();
            services.AddSingleton<IConnectionFactory, SqlConnectionFactory>();

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
                        //options.Events = new JwtBearerEvents
                        //{
                        //    OnChallenge = context =>
                        //    {
                        //        if (!context.Handled)
                        //        {
                        //            context.Response.StatusCode = 401;
                        //        }
                        //        return Task.FromResult<object>(0);
                        //    }
                        //};
                        //options.SecurityTokenValidators.Add(new JwtValidator(new SystemClock()));
                    });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddNLog();
            app.AddNLogWeb();

            app.UseAuthentication();
            //app.UseExceptionHandler(appBuilder =>
            //{
            //    appBuilder.Use(async (context, next) =>
            //    {
            //        var error = context.Features[typeof(IExceptionHandlerFeature)] as IExceptionHandlerFeature;

            //        //when authorization has failed, should retrun a json message to client
            //        if (error?.Error is SecurityTokenExpiredException)
            //        {
            //            context.Response.StatusCode = 401;
            //            context.Response.ContentType = "application/json";

            //            await context.Response.WriteAsync(JsonConvert.SerializeObject(new {State = 401, Msg = "token invalid"}));
            //        }
            //        //when other error, retrun a error message json to client
            //        else if (error?.Error != null)
            //        {
            //            context.Response.StatusCode = 500;
            //            context.Response.ContentType = "application/json";
            //            await context.Response.WriteAsync(JsonConvert.SerializeObject(new {State = 500, Msg = error.Error.Message}.ToString()));
            //        }
            //        //when no error, do next.
            //        else await next();
            //    });
            //});

            app.UseMvc();
        }
    }
}


//todo: 
//finish adding Nlog
//Customise git(gitconfig / aliases)