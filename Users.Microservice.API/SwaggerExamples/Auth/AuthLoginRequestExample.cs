using Swashbuckle.AspNetCore.Filters;
using Users.Microservice.Application.Auth.Commands;

namespace Fiap.Api.SwaggerExamples.Auth
{
    
    public class AuthLoginRequestExample : IExamplesProvider<LoginCommand>
    {
        public LoginCommand GetExamples()
        {
            return new LoginCommand
            {
                Email = "adminnew@fiapgames.com",
                Password = "1GamesAdmin@"
            };
        }
    }
}
