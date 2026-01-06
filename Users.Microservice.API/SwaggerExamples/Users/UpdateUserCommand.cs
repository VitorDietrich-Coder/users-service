using Users.Microservice.Domain.Entities.Enums;

namespace Users.Microservice.API.SwaggerExamples.Users
{
    internal class UpdateUserCommand
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public UserType TypeUser { get; set; }
        public bool Active { get; set; }
    }
}