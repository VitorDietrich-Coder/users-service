using MediatR;
using Users.Microservice.Application.Commands;
using Users.Microservice.Domain.Entities;
using Users.Microservice.Domain.Interfaces;

namespace Users.Microservice.Application.Handlers;

public class CreateUserHandler : IRequestHandler<CreateUserCommand, Guid>
{
    private readonly IUserRepository _repository;

    public CreateUserHandler(IUserRepository repository)
    {
        _repository = repository;
    }

    public async Task<Guid> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var user = new User(
            request.Name,
            request.Email,
            BCrypt.Net.BCrypt.HashPassword(request.Password)
        );

        await _repository.AddAsync(user);
        return user.Id;
    }
}
