using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Users.Microservice.Domain.Core.Events;

namespace Users.Microservice.Domain.Events
{
    public class UpdateUsernameEvent : DomainEvent
    {
        public UpdateUsernameEvent(Guid aggregateId)
            : base(aggregateId)
        {
        }
    }
}
