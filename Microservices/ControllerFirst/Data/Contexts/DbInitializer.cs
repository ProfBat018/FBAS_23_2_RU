using Bogus;
using ControllerFirst.Data.Models;

namespace ControllerFirst.Contexts;

public class DbInitializer
{
    public static void SeedDatabase(AuthContext context)
    {

            var userFaker = new Faker<User>()
                .RuleFor(u => u.Id, f => Guid.NewGuid())
                .RuleFor(u => u.UserName, f => f.Internet.UserName())
                .RuleFor(u => u.Password, f => f.Internet.Password())
                .RuleFor(u => u.Email, f => f.Internet.Email())
                .RuleFor(u => u.IsEmailConfirmed, f => f.Random.Bool())
                .RuleFor(u => u.RefreshToken, f => Guid.NewGuid())
                .RuleFor(u => u.RefreshTokenExpiration, f => DateTime.Now.AddDays(7));

            var users = userFaker.Generate(20);


            var userRoles = users.Select(user => new UserRole
            {
                UserRoleId = Guid.NewGuid(),
                UserRef = user.Id,
                RoleNameRef = "AppUser",
                UserNameRefNavigation = user
            }).ToList();
            
            context.Users.AddRange(users);
            context.UserRoles.AddRange(userRoles);

            context.SaveChanges();
    }
}