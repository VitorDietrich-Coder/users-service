using Microsoft.EntityFrameworkCore;
using Users.Microservice.Domain.Entities.Users;
using Users.Microservice.Infrastructure.EventStore;


namespace FGC.Application.Common;

public interface IApplicationDbContext
{
    public DbSet<User> Users { get; }
 
    public DbSet<StoredEvent> StoredEvents { get; }

 
}

