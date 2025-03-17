using Serilog;
using Serilog.Sinks.Grafana.Loki;

namespace CRM.API.Configurations
{
    public static class LoggingConfig
    {
        public static IHostBuilder ConfigureLogging(this IHostBuilder builder)
        {
            return builder.UseSerilog((context, services, configuration) =>
            {
                configuration
                    .ReadFrom.Configuration(context.Configuration)
                    .ReadFrom.Services(services)
                    .Enrich.FromLogContext()
                    .WriteTo.Console()
                    .WriteTo.GrafanaLoki(
                        "http://localhost:3100",
                        labels: new[] {
                            new LokiLabel { Key = "app", Value = "crm-api" },
                            new LokiLabel { Key = "environment", Value = context.HostingEnvironment.EnvironmentName }
                        });
            });
        }
    }
}
