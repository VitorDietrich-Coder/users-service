using MediatR;
using Microsoft.EntityFrameworkCore;
using Users.Microservice.Application.Queries;
using Users.Microservice.Application.Users.Models.Response;
using Users.Microservice.Infrastructure.Persistence;

namespace Users.Microservice.Application.Handlers.Query
{
    public class GetAllUsersQueryHandler
        : IRequestHandler<GetAllUsersQuery, List<UserResponse>>
    {
        private readonly UsersDbContext _context;

        public GetAllUsersQueryHandler(UsersDbContext context)
        {
            _context = context;
        }

        public async Task<List<UserResponse>> Handle(
            GetAllUsersQuery request,
            CancellationToken cancellationToken)
        {
            return await _context.Users
                .AsNoTracking()
                .OrderBy(u => u.CreatedAt)
                .Select(u => new UserResponse
                {
                    UserId = u.Id,
                    Name = u.Name,
                    Email = u.Email.Address,
                    Username = u.Username,
                    Type = u.TypeUser,
                    Active = u.Active,
                 })
                .ToListAsync(cancellationToken);
        }
    }
}
