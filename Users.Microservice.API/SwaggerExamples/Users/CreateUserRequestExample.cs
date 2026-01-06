using Swashbuckle.AspNetCore.Filters;
using Users.Microservice.Application.Commands;

namespace FGC.Api.SwaggerExamples.Users
{ 
    public class CreateUserAdminRequestExample : IExamplesProvider<CreateAdminCommand>
    {
        public CreateAdminCommand GetExamples()
        {
            return new CreateAdminCommand
            {
                Name = "User",
                Username = "User Gameplays",
                Email = "user@hotmail.com",
                Password = "Password123",
            };
        }
    }
}
