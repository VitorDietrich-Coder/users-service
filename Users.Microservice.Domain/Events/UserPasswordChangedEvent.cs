using Users.Microservice.Domain.Core.Events;
using Users.Microservice.Domain.Entities.ValueObjects;

namespace Users.Microservice.Domain.Events;

public class UserPasswordChangedEvent : DomainEvent
{
    public Password Password { get; }
    public UserPasswordChangedEvent(Guid aggregateId, Password password)
        : base(aggregateId)
    {
        Password = password;
    }
}
