using MediatR;
 
namespace Users.Microservice.Application.Commands
{
    public record CreateUserCommand(
        string Name,
        string Email,
        string Password
    ) : IRequest<Guid>;

}
