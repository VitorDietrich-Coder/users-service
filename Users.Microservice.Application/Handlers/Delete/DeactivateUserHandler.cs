using MediatR;
using Users.Microservice.Application.Commands;
using Users.Microservice.Domain.Interfaces;
using Users.Microservice.Infrastructure.Interfaces;
using Users.Microservice.Infrastructure.Persistence;

namespace Users.Microservice.Application.Handlers.Delete;

public class DeactivateUserHandler
    : IRequestHandler<DeactivateUserCommand>
{
    private readonly IUserRepository _repository;
    private readonly IEventStore _eventStore;
    private readonly IEventBus _eventBus;
    private readonly IUnitOfWork _unitOfWork;

    public DeactivateUserHandler(
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
        DeactivateUserCommand request,
        CancellationToken cancellationToken)
    {
        var user = await _repository.GetByIDAsync(request.UserId);

        if (user is null)
            throw new Exception("Usuário não encontrado");

        user.Deactivate();

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
