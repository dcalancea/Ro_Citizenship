﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WebApplication1.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using WebApplication1.Repository;
using WebApplication1.Checker;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Routing;
using System.IO;

namespace WebApplication1
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            if (env.IsEnvironment("Development"))
            {
                // This will push telemetry data through Application Insights pipeline faster, allowing you to view results immediately.
                builder.AddApplicationInsightsSettings(developerMode: true);
            }

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container
        public void ConfigureServices(IServiceCollection services)
        {
            //JsonConvert.DefaultSettings = (() =>
            //{
            //    var settings = new JsonSerializerSettings();
            //    // do something with settings
            //    return settings;
            //});

            // Add framework services.
            services.AddApplicationInsightsTelemetry(Configuration);

            services.AddMvc();

            //services.AddJsonOptions(options =>
            //{
            //    var settings = options.SerializerSettings;
            //    // do something with settings
            //});

            services.AddOptions();
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));
            services.AddSingleton<IChecker, Checker.Checker>();
            services.AddSingleton<IUserRepository, UserRepository>();

            services.AddEntityFramework()
                //      .AddEntityFrameworkSqlServer()
                .AddDbContext<DbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddDbContext<PlayGroundContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IChecker checker)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            app.Use(async (context, next) =>
            {
                await next();

                if (context.Response.StatusCode == 404
                    && !Path.HasExtension(context.Request.Path.Value))
                {
                    context.Request.Path = "/index.html";
                    await next();
                }
            });
            app.UseStaticFiles();
            app.UseApplicationInsightsRequestTelemetry();

            app.UseApplicationInsightsExceptionTelemetry();

            app.UseMvc(routes =>
            {

                routes.MapRoute(
                    name: "api_route",
                    template: "api/{controller}/{action}/{id?}",
                    defaults: new { controller = "Values", action = "Get" });

                //routes.MapRoute(
                //    name: "default_route",
                //    template: "",
                //    defaults: "index.html"
                //    );
            });
        }
    }
}
