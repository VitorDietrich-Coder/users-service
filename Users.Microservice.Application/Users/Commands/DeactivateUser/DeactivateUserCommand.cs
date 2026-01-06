using MediatR;

namespace Users.Microservice.Application.Commands;

public record DeactivateUserCommand(Guid UserId) : IRequest;
