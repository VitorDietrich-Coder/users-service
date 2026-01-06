using MediatR;
using Users.Microservice.Domain.Entities.Enums;

namespace Users.Microservice.Application.Commands;

public record CreateUserCommand(
    string Name,
    string Email,
    string Password,
    string Username,
    UserType TypeUser
) : IRequest<Guid>;
