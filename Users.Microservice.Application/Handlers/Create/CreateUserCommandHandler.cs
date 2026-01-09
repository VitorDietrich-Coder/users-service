using MediatR;
using Microsoft.Extensions.Logging;
using Users.Microservice.Application.Commands;
using Users.Microservice.Domain.Entities.Enums;
using Users.Microservice.Domain.Entities.Users;
using Users.Microservice.Domain.Events;
using Users.Microservice.Domain.Interfaces;
using Users.Microservice.Infrastructure.Interfaces;
using Users.Microservice.Infrastructure.Persistence;

namespace Users.Microservice.Application.Handlers.Create;

public class CreateUserCommandHandler
    : IRequestHandler<CreateUserCommand, Guid>
{
    private readonly IUserRepository _repository;
    private readonly IEventStore _eventStore;
    private readonly IEventBus _eventBus;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CreateUserCommandHandler> _logger;

    public CreateUserCommandHandler(
        IUserRepository repository,
        IEventStore eventStore,
        IUnitOfWork unitOfWork,
        IEventBus eventBus,
        ILogger<CreateUserCommandHandler> logger)
    {
        _repository = repository;
        _eventStore = eventStore;
        _eventBus = eventBus;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Guid> Handle(
        CreateUserCommand request,
        CancellationToken cancellationToken)
    {
        var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

        var user = new User(
            request.Name,
            request.Email,
            passwordHash,
            UserType.User,
            request.Username);

        await _repository.AddAsync(user);

        try
        {
            foreach (var domainEvent in user.DomainEvents)
            {
                await _eventStore.SaveAsync(domainEvent);

                if (domainEvent is UserCreatedEvent userCreated)
                {
                    await _eventBus.PublishAsync(userCreated, "users.events");
                }
            }

            await _unitOfWork.CommitAsync(cancellationToken);
       

        user.ClearDomainEvents();
        }
        catch(Exception ex)
        {
            _logger.LogError("Error of publish" + ex);
        }
        return user.Id;
    }
}
