using MediatR;
using Microsoft.EntityFrameworkCore;
using Users.Microservice.Application.Queries;
using Users.Microservice.Application.Users.Models.Response;
using Users.Microservice.Infrastructure.Persistence;

namespace Users.Microservice.Application.Handlers.Query
{
    public class GetUserByIdQueryHandler
        : IRequestHandler<GetUserByIdQuery, UserResponse>
    {
        private readonly UsersDbContext _context;

        public GetUserByIdQueryHandler(UsersDbContext context)
        {
            _context = context;
        }

        public async Task<UserResponse> Handle(
            GetUserByIdQuery request,
            CancellationToken cancellationToken)
        {
            var user = await _context.Users
                .AsNoTracking()
                .Where(u => u.Id == request.Id)
                .Select(u => new UserResponse
                {
                    UserId = u.Id,
                    Name = u.Name,
                    Email = u.Email.Address,
                    Username = u.Username,
                    Type = u.TypeUser,
                    Active = u.Active,
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (user is null)
                throw new KeyNotFoundException("User not found");

            return user;
        }
    }
}
