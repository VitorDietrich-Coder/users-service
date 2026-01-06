using Microsoft.EntityFrameworkCore;
using Users.Microservice.Domain.Core.Events;
using Users.Microservice.Domain.Entities.Users;
using Users.Microservice.Infrastructure.EventStore;
using Users.Microservice.Infrastructure.MapEntities;


namespace Users.Microservice.Infrastructure.Persistence
{
    public class UsersDbContext : DbContext
    {
        public DbSet<User> Users => Set<User>();
        public DbSet<StoredEvent> StoredEvents => Set<StoredEvent>();

        public UsersDbContext(DbContextOptions<UsersDbContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Ignore<DomainEvent>();
            modelBuilder.Ignore<List<DomainEvent>>();
            modelBuilder.Ignore<IReadOnlyCollection<DomainEvent>>();

            modelBuilder.ApplyConfigurationsFromAssembly(
                typeof(UserConfiguration).Assembly
            );

        }
    }
}
