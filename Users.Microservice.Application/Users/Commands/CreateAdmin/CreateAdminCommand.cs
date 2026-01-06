using MediatR;

namespace Users.Microservice.Application.Commands
{
    public class CreateAdminCommand : IRequest<Guid>
    {
        public string Name { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string Password { get; set; } = default!;
        public string Username { get; set; } = default!;
    }
}
