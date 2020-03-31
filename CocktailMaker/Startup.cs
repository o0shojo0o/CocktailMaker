using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FluentScheduler;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace CocktailMaker
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            // Conifg mit den übergeben Enviroments füllen.
            Globe.Config = Mapping.DictionaryToObject<Config>(configuration.AsEnumerable().ToDictionary(x => x.Key, x => x.Value));

            // Serilog 
            LoggerConfiguration serilogConfig = new LoggerConfiguration();
            serilogConfig = serilogConfig.Enrich.FromLogContext();
            serilogConfig = serilogConfig.Enrich.With<LogEnricher>();
            serilogConfig = serilogConfig.WriteTo.ColoredConsole();

            // an RollingFile senden ?
            if (Globe.Config.LOG_TO_FILE == true)
            {
                serilogConfig = serilogConfig.WriteTo.RollingFile("/app/logs/log-{Date}.txt");
            }

            // an Seq senden ?    
            if (Globe.Config.LOG_TO_SEQ == true && !String.IsNullOrWhiteSpace(Globe.Config.SEQ_SERVER_ADDRESS))
            {
                serilogConfig = serilogConfig.WriteTo.Seq(Globe.Config.SEQ_SERVER_ADDRESS, apiKey: Globe.Config.SEQ_API_KEY);
            }

            // und finale Instanz setzen
            Log.Logger = serilogConfig.CreateLogger();

            Log.Information("Starting Jobs scheduler...");

            // Geplante Jobs laden ..
            JobManager.Initialize(new ScheduledJobs());

            // Ausgabe welche Jobs geladen wurden
            foreach (var x in JobManager.AllSchedules)
            {
                Log.Information("Job {JobName} started", x.Name);
            }

            // Fertig geladen
            Log.Information("Cocktail Maker started!");
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            // Für HttpContext im Enricher
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Nötige das die Client IP-Adresse auch greifbar ist.
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.All,
                ForwardLimit = null, // null = disable check
                RequireHeaderSymmetry = false,
                KnownProxies = { IPAddress.Parse("172.18.0.1"), IPAddress.Parse("172.19.0.1"), IPAddress.Parse("172.20.0.1"), IPAddress.Parse("127.0.0.1") },
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}