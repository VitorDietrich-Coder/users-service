using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Users.Microservice.Domain.Core.Events;

namespace Users.Microservice.Domain.Events
{
    public class UpdateNameEvent : DomainEvent
    {
        public string Name { get; set; }
        public UpdateNameEvent(Guid aggregateId, string name)
            : base(aggregateId)
        {
            Name = name;
        }
    }
}
