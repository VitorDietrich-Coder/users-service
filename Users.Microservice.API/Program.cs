using Amazon.SimpleNotificationService;
using Microsoft.EntityFrameworkCore;
using Prometheus;
using Serilog;
using Users.Microservice.API.Extensions;
using Users.Microservice.API.Middlewares;
using Users.Microservice.Domain.Interfaces;
using Users.Microservice.Infrastructure.EventStore;
using Users.Microservice.Infrastructure.Interfaces;
using Users.Microservice.Infrastructure.Messaging;
using Users.Microservice.Infrastructure.Persistence;
using Users.Microservice.Infrastructure.Repositories;
 
var builder = WebApplication.CreateBuilder(args);


builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddEnvironmentVariables();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<UsersDbContext>(options =>
    options.UseSqlServer(connectionString),
    ServiceLifetime.Scoped);


builder.Services.AddScoped<ApplicationDbContextInitialiser>();

 
builder.Services.AddOpenTelemetryTracing(builder.Configuration);
builder.Services.AddApiVersioningConfiguration();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerServices();

builder.Services.AddGlobalCorsPolicy();
builder.Services.AddHttpContextAccessor();

builder.Services.AddAWSService<IAmazonSimpleNotificationService>();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddHealthChecks();

builder.Services.AddApplication();

builder.Services.AddAuthorization();


builder.Host.UseSerilog((context, services, configuration) =>
{
    SerilogExtensions.ConfigureSerilog(context, services, configuration);
});
builder.Services.AddCustomAuthentication(builder.Configuration);

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IEventStore, StoredEvents>();
builder.Services.AddSingleton<IEventBus>(sp =>
{
     return new RabbitMqEventBus(builder.Configuration);
});
var app = builder.Build();

app.UseMiddleware<LoggingMiddleware>();
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseMiddleware<CorrelationIdMiddleware>();

using (var scope = app.Services.CreateScope())
{
    try
    {
        var initialiser = scope.ServiceProvider.GetRequiredService<ApplicationDbContextInitialiser>();
        initialiser.Initialise();
        initialiser.Seed();
    }
    catch (Exception ex)
    {
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred during database initialisation.");

        throw;
    }

}
 

//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI(options =>
//    {
//        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Users API v1");

//        options.RoutePrefix = string.Empty;
//    });
//}

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Users API v1");

    options.RoutePrefix = string.Empty;
});

app.UseRouting();
app.UseHttpMetrics();
app.UseHttpsRedirection();


app.MapHealthChecks("/health");

app.UseAuthentication();
app.UseMiddleware<UnauthorizedResponseMiddleware>();
app.UseAuthorization();

app.MapControllers();

app.Run();
