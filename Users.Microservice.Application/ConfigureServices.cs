using FluentValidation;
using MediatR;
using Users.Microservice.Application.Behaviours;
using Users.Microservice.Infrastructure.Persistence;

namespace Microsoft.Extensions.DependencyInjection;

public static class ConfigureServices
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        //services.AddAutoMapper(Assembly.GetExecutingAssembly());

        services.AddValidatorsFromAssemblyContaining<UsersDbContext>();
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(ApplicationAssemblyMarker).Assembly);
        });
        services.AddTransient(typeof(IPipelineBehavior<,>),
            typeof(UnhandledExceptionBehaviour<,>));

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(CorrelationBehavior<,>));

        services.AddTransient(typeof(IPipelineBehavior<,>),
            typeof(ValidationBehaviour<,>));

        return services;
    }

}
public sealed class ApplicationAssemblyMarker { }



