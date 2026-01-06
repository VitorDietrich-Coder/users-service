namespace Users.Microservice.Application.Auth.Models.Response;

public class LoginResponse
{
    public string Token { get; set; } = null!;
    public DateTime Expiration { get; set; }
}
