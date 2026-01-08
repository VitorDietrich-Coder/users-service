using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace Users.Microservice.API.Extensions
{
    public static class OpenTelemetryExtensions
    {
        public static IServiceCollection AddOpenTelemetryTracing(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var serviceName =
                configuration["Service:Name"] ?? "users-microservice";

            services.AddOpenTelemetry()
                .WithTracing(builder =>
                {
                    builder
                        .SetResourceBuilder(
                            ResourceBuilder.CreateDefault()
                                .AddService(serviceName))

                        .AddAspNetCoreInstrumentation(options =>
                        {
                            options.RecordException = true;
                        })

                        .AddHttpClientInstrumentation()

                        .AddEntityFrameworkCoreInstrumentation()

                    
                        .AddOtlpExporter(opt =>
                        {
                            opt.Endpoint = new Uri(
                                configuration["Jaeger:OtlpEndpoint"]
                                ?? "http://jaeger:4317");
                        });
                });

            return services;
        }
    }
}
