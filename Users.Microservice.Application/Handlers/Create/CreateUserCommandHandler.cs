using MediatR;
using Users.Microservice.Application.Commands;
using Users.Microservice.Domain.Entities.Enums;
using Users.Microservice.Domain.Entities.Users;
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

    public CreateUserCommandHandler(
        IUserRepository repository,
        IEventStore eventStore,
        IUnitOfWork unitOfWork,
        IEventBus eventBus)
    {
        _repository = repository;
        _eventStore = eventStore;
        _eventBus = eventBus;
        _unitOfWork = unitOfWork;
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


        foreach (var domainEvent in user.DomainEvents)
        {
            await _eventStore.SaveAsync(domainEvent);
            await _eventBus.PublishAsync(domainEvent, "users.events");
        }
        await _unitOfWork.CommitAsync(cancellationToken);

        user.ClearDomainEvents();

        return user.Id;
    }
}
