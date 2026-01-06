using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Users.Microservice.Domain.Entities;
using Users.Microservice.Domain.Entities.Users;


namespace Users.Microservice.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetByIDAsync(Guid idUser);
        Task<User?> UpdateAsync(User user);
        Task AddAsync(User user);
    }
}
