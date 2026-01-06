using Microsoft.EntityFrameworkCore;
using Users.Microservice.Domain.Entities;
using Users.Microservice.Domain.Entities.Users;
using Users.Microservice.Domain.Interfaces;
using Users.Microservice.Infrastructure.Persistence;

namespace Users.Microservice.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UsersDbContext _context;

        public UserRepository(UsersDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await Task.CompletedTask;  
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Email.Address == email);
        }

        public async Task<User?> GetByIDAsync(Guid idUser)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Id == idUser);

        }

        public async Task<User?> UpdateAsync(User user)
        {
            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == user.Id);

            if (existingUser is null)
                return null;

            _context.Entry(existingUser).CurrentValues.SetValues(user);

            return existingUser;
        }
    }
}
