using Users.Microservice.Domain.Entities.Enums;
using Users.Microservice.Domain.Entities.Users;

namespace Users.Microservice.Application.Users.Models.Response
{
    
    public record UserResponse
    {
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public UserType Type { get; set; }
        public bool Active { get; set; }

        public static explicit  operator UserResponse(User user)
        {
            return new UserResponse
            {
                UserId = user.Id,
                Name = user.Name,
                Username = user.Username,
                Email = user.Email.Address,
                Type = user.TypeUser,
                Active = user.Active,
            };
        }
    }   
}





