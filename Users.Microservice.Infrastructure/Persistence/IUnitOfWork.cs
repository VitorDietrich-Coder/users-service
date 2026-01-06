using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Users.Microservice.Infrastructure.Persistence
{
    public interface IUnitOfWork
    {
        Task CommitAsync(CancellationToken ct);
    }
}
