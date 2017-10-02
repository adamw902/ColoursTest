using ColoursTest.AppServices.Interfaces;
using ColoursTest.AppServices.Services;
using ColoursTest.Infrastructure.Factories;
using ColoursTest.Domain.Interfaces;
using ColoursTest.Infrastructure.Interfaces;
using ColoursTest.Infrastructure.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ColoursTest.Web
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
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
            services.AddSingleton<IConnectionFactory, SqlConnectionFactory>();

            services.AddTransient<IPersonRepository, PersonRepository>();
            services.AddTransient<IColourRepository, ColourRepository>();
            services.AddTransient<IPersonService, PersonService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseMvc();
        }
    }
}


//todo: 
//Enable reshaper, with coding standards(.settings)
//Add Nlog(via ILogger interface)
//Customise git(gitconfig / aliases)
//Presentation on SOLID principles for (next Monday 2pm)