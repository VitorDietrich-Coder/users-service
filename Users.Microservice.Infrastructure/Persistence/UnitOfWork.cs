using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Users.Microservice.Infrastructure.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly UsersDbContext _context;

        public UnitOfWork(UsersDbContext context)
        {
            _context = context;
        }

        public async Task CommitAsync(CancellationToken ct)
        {
            await _context.SaveChangesAsync(ct);
        }
    }
}
