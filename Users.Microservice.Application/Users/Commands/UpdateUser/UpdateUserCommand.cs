using MediatR;

namespace Users.Microservice.Application.Commands;

public record UpdateUserCommand(
    Guid UserId,
    string NewPassword,
    string Name,
    string Username
) : IRequest;
