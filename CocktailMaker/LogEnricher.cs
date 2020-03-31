using Microsoft.AspNetCore.Http;
using Serilog.Core;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CocktailMaker
{
    public class LogEnricher : ILogEventEnricher
    {
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            logEvent.AddOrUpdateProperty(propertyFactory.CreateProperty("Application", "CocktailMaker"));
            logEvent.AddOrUpdateProperty(propertyFactory.CreateProperty("MachineName", Environment.MachineName.ToLower()));

            // Log für Webanwendung
            var httpContext = new HttpContextAccessor().HttpContext;
            if (httpContext != null)
            {
                var request = httpContext.Request;
                var rawURL = $"{request.HttpContext.Request.Scheme}://{request.HttpContext.Request.Host}{request.HttpContext.Request.Path}{request.HttpContext.Request.QueryString}";

                logEvent.AddOrUpdateProperty(propertyFactory.CreateProperty("SourceIp", request.HttpContext.Connection.RemoteIpAddress));
                logEvent.AddOrUpdateProperty(propertyFactory.CreateProperty("UserAgent", request.HttpContext.Request.Headers["User-Agent"]));
                logEvent.AddOrUpdateProperty(propertyFactory.CreateProperty("RawUrl", rawURL));
            }
        }
    }
}