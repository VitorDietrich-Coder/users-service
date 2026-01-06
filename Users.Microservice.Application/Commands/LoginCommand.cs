using MediatR;
using Users.Microservice.Application.Auth.Models.Response;

namespace Users.Microservice.Application.Auth.Commands;

public class LoginCommand : IRequest<LoginResponse>
{
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
}
