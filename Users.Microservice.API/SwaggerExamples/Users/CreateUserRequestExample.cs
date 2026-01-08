using Swashbuckle.AspNetCore.Filters;
using Users.Microservice.Application.Commands;
using Users.Microservice.Domain.Entities.Enums;

namespace FGC.Api.SwaggerExamples.Users
{ 
    public class CreateUserAdminRequestExample : IExamplesProvider<CreateAdminCommand>
    {
        public CreateAdminCommand GetExamples()
        {
            return new CreateAdminCommand("User", "user@hotmail.com",  "Password123", "User Gameplays", UserType.User);
        }
    }
}
