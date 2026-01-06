using Users.Microservice.Domain.Core.Events;

namespace Users.Microservice.Domain.Interfaces
{
    public interface IEventStore
    {
        Task SaveAsync(DomainEvent @event);
        Task<IEnumerable<DomainEvent>> GetEventsAsync(Guid aggregateId);
    }
}
