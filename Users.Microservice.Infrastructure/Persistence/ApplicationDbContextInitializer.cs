
using Microsoft.EntityFrameworkCore;
using Users.Microservice.Domain.Entities.Enums;
using Users.Microservice.Domain.Entities.Users;
using Users.Microservice.Domain.Entities.ValueObjects;

namespace Users.Microservice.Infrastructure.Persistence;

public class ApplicationDbContextInitialiser
{
    private readonly UsersDbContext _context;

    public ApplicationDbContextInitialiser(UsersDbContext context)
    {
        _context = context;
    }

    public  void Initialise()
    {
        // Early development strategy
        //_context.Database.EnsureDeleted();
        //_context.Database.EnsureCreated();
        //_context.Database.Migrate();

        // Late development strategy
        if (_context.Database.IsSqlServer())
        {
             _context.Database.Migrate();
        }
        else
        {
            _context.Database.EnsureCreated();
        }
    } 

    public void Seed()
    {    
        SeedUsers(); 
    }

    private void SeedUsers()
    {
        if (_context.Users.Any())
            return;

        if (_context.Database.IsSqlServer())
        {
            using var transaction = _context.Database.BeginTransaction();


            var users = GetUsers();
            _context.Users.AddRange(users);
            _context.SaveChanges();


            transaction.Commit();
        }
        else
        {
            var users = GetUsers();
            _context.Users.AddRange(users);
            _context.SaveChanges();
        }
    }



    public static List<User> GetUsers()
    {
        return new List<User>
        {
                new User
                {
           
                    Name = "Admin New",
                    Username = "adminnew",
                    TypeUser = UserType.Admin,
                    Active = true,
                    Email = new Email("adminnew@fiapgames.com"),
                    Password = new Password("1GamesAdmin@"),
                    CreatedAt = new DateTime(2025, 8, 2, 20, 0, 0, DateTimeKind.Utc)

                },
                new User
                {
           
                    Name = "User New",
                    Username = "usernew",
                    Email = new Email("usernew@fiapgames.com"),
                    Password =  new Password("1GamesTeste@"),
                    TypeUser = UserType.User,
                    Active = true,
                    CreatedAt = new DateTime(2025, 8, 2, 20, 0, 0, DateTimeKind.Utc)
                }
        };
    }
}
