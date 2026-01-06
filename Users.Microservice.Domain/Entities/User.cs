using Users.Microservice.Domain.Core;
using Users.Microservice.Domain.Core.Events;
using Users.Microservice.Domain.Entities.Enums;
using Users.Microservice.Domain.Entities.ValueObjects;
using Users.Microservice.Domain.Events;


namespace Users.Microservice.Domain.Entities.Users
{
    public class User : Entity
    {
        public User() { }


        public User(
            string name,
            string email,
            string password,
            UserType typeUser,
            string username)
        {
            Id = Guid.NewGuid();
            Name = name;
            Email = new Email(email);
            Password = new Password(password);
            TypeUser = typeUser;
            Username = username;
            Active = true;
            CreatedAt = DateTime.UtcNow;

            AddDomainEvent(new UserCreatedEvent(
              Id,  Name, Email.Address, Username, TypeUser));
        }


        public string Name { get;  set; }
        public Email Email { get;  set; }
        public Password Password { get;  set; }
        public UserType TypeUser { get;  set; }
        public bool Active { get;  set; }
        public string Username { get;  set; }
        public DateTime CreatedAt { get;  set; }
        public DateTime? UpdatedAt { get; set; }

        public void Deactivate()
        {
            Active = false;
            UpdatedAt = DateTime.UtcNow;

            AddDomainEvent(new UserDeactivatedEvent(Id, false));
        }

        public void ChangePassword(string newPassword)
        {
            Password = new Password(newPassword);
            UpdatedAt = DateTime.UtcNow;

            AddDomainEvent(new UserPasswordChangedEvent(Id, Password));
        }

        public void UpdateName(string name)
        {
            Name = name;
            UpdatedAt = DateTime.UtcNow;
            AddDomainEvent(new UpdateNameEvent(Id, name));
        }

        public void UpdateUsername(string username)
        {
            Username = username;
            UpdatedAt = DateTime.UtcNow;
            AddDomainEvent(new UpdateUsernameEvent(Id));

        }
     }
}
