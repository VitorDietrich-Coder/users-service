using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Authentication;
using System.Text;
using Users.Microservice.Application.Auth.Models.Response;
using Users.Microservice.Domain.Entities.Users;
using Users.Microservice.Domain.Interfaces;
using Users.Microservice.Application.Auth.Commands;

namespace Users.Microservice.Application.Handlers.Auth;

public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _configuration;

    public LoginCommandHandler(
        IUserRepository userRepository,
        IConfiguration configuration)
    {
        _userRepository = userRepository;
        _configuration = configuration;
    }

    public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var email = request.Email.Trim().ToLowerInvariant();

        var user = await _userRepository.GetByEmailAsync(email);

        if (user is null || !user.Active)
            throw new AuthenticationException("Invalid credentials.");

        if (!user.Password.Challenge(request.Password))
            throw new AuthenticationException("Invalid credentials.");

        var (token, expiration) = GenerateJwtToken(user);

        return new LoginResponse
        {
            Token = token,
            Expiration = expiration
        };
    }

    private (string Token, DateTime Expiration) GenerateJwtToken(User user)
    {
        var secretKey = _configuration["JwtSettings:SecretKey"];
        var issuer = _configuration["JwtSettings:Issuer"];
        var audience = _configuration["JwtSettings:Audience"];

        if (string.IsNullOrWhiteSpace(secretKey))
            throw new InvalidOperationException("JWT SecretKey not configured.");

        var key = Encoding.UTF8.GetBytes(secretKey);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),          // 🔑 padrão
            new(JwtRegisteredClaimNames.Email, user.Email.Address),
            new(ClaimTypes.Role, user.TypeUser.ToString())
        };

        var expiration = DateTime.UtcNow.AddHours(2);

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: expiration,
            signingCredentials: new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256)
        );

        return (new JwtSecurityTokenHandler().WriteToken(token), expiration);
    }
}
