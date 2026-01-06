using MediatR;
using Users.Microservice.Application.Commands;
using Users.Microservice.Domain.Interfaces;
using Users.Microservice.Infrastructure.Interfaces;
using Users.Microservice.Infrastructure.Persistence;

namespace Users.Microservice.Application.Handlers.Update;

public class UpdateUserCommandHandler
    : IRequestHandler<UpdateUserCommand>
{
    private readonly IUserRepository _repository;
    private readonly IEventStore _eventStore;
    private readonly IEventBus _eventBus;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateUserCommandHandler(
        IUserRepository repository,
        IEventStore eventStore,
        IEventBus eventBus,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _eventStore = eventStore;
        _eventBus = eventBus;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(
        UpdateUserCommand request,
        CancellationToken cancellationToken)
    {
        var user = await _repository.GetByIDAsync(request.UserId);

        if (user is null)
            throw new Exception("Usuário não encontrado");

        if (!string.IsNullOrWhiteSpace(request.Name))
        {
            user.UpdateName(request.Name);
        }

        if (!string.IsNullOrWhiteSpace(request.Username))
        {
            user.UpdateUsername(request.Username);
        }

        if (!string.IsNullOrWhiteSpace(request.NewPassword))
        {
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
            user.ChangePassword(passwordHash);
        }

        await _repository.UpdateAsync(user);

        foreach (var domainEvent in user.DomainEvents)
        {
            await _eventStore.SaveAsync(domainEvent);
            await _eventBus.PublishAsync(domainEvent, "users.events");
        }

        await _unitOfWork.CommitAsync(cancellationToken);

        user.ClearDomainEvents();
    }
}

