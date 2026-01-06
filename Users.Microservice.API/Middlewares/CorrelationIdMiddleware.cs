using Serilog.Context;
using System.Diagnostics;
using Users.Microservice.Application.Behaviours;
using Users.Microservice.Shared;

namespace Users.Microservice.API.Middlewares
{
    public class CorrelationIdMiddleware
    {
        private const string HeaderName = "X-Correlation-Id";
        private readonly RequestDelegate _next;

        public CorrelationIdMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if (!context.Request.Headers.TryGetValue(HeaderName, out var correlationId))
            {
                correlationId = Guid.NewGuid().ToString();
            }

            CorrelationContext.Current = correlationId!;

            context.Response.Headers[HeaderName] = correlationId!;

            await _next(context);
        }
    }
}
