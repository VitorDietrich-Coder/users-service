using MediatR;
using Users.Microservice.Application.Users.Models.Response;

namespace Users.Microservice.Application.Queries
{
    public record GetAllUsersQuery : IRequest<List<UserResponse>>;
}
