using Users.Microservice.Domain.Core.Events;
using Users.Microservice.Domain.Entities.Enums;

namespace Users.Microservice.Domain.Events;

public class UserDeactivatedEvent : DomainEvent
{
    public bool Active { get; }
    public UserDeactivatedEvent(Guid aggregateId, bool active)
        : base(aggregateId)
    {
        Active = active;
    }
}
 