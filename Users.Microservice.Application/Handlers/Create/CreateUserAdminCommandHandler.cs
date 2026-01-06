using MediatR;
using Users.Microservice.Application.Commands;
using Users.Microservice.Domain.Entities.Enums;
using Users.Microservice.Domain.Entities.Users;
using Users.Microservice.Domain.Interfaces;
using Users.Microservice.Infrastructure.Interfaces;

namespace Users.Microservice.Application.Handlers.Create
{
    public class CreateUserAdminCommandHandler
        : IRequestHandler<CreateAdminCommand, Guid>
    {
        private readonly IUserRepository _userRepository;
        private readonly IEventStore _eventStore;
        private readonly IEventBus _eventBus;

        public CreateUserAdminCommandHandler(
            IUserRepository userRepository,
            IEventStore eventStore,
            IEventBus eventBus)
        {
            _userRepository = userRepository;
            _eventStore = eventStore;
            _eventBus = eventBus;
        }

        public async Task<Guid> Handle(
            CreateAdminCommand request,
            CancellationToken cancellationToken)
        {
            var user = new User(
                name: request.Name,
                email: request.Email,
                password: request.Password,
                typeUser: UserType.Admin,
                username: request.Username
            );

 
            foreach (var domainEvent in user.DomainEvents)
            {
                await _eventStore.SaveAsync(domainEvent);
                await _eventBus.PublishAsync(domainEvent, "users.events");
            }

            user.ClearDomainEvents();

            await _userRepository.AddAsync(user);

            return user.Id;
        }
    }
}
