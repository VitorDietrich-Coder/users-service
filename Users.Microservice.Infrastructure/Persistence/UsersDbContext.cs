using Microsoft.EntityFrameworkCore;
using Users.Microservice.Domain.Entities;


namespace Users.Microservice.Infrastructure.Persistence
{
    public class UsersDbContext : DbContext
    {
        public DbSet<User> Users => Set<User>();

        public UsersDbContext(DbContextOptions<UsersDbContext> options)
            : base(options) { }
    }
}
