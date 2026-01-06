using Users.Microservice.Domain.Core.Events;
using Users.Microservice.Domain.Entities.Enums;

namespace Users.Microservice.Domain.Events
{
    public class UserCreatedEvent : DomainEvent
    {
        public string Name { get; }
        public string Email { get; }
        public string Username { get; }
        public UserType TypeUser { get; }

        public UserCreatedEvent(
            Guid aggregateId,
            string name,
            string email,
            string username,
            UserType typeUser) : base(aggregateId)
        {
            Name = name;
            Email = email;
            Username = username;
            TypeUser = typeUser;
        }
    }
}
