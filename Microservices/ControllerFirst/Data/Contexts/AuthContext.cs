using ControllerFirst.Data.Models;

using Microsoft.EntityFrameworkCore;

namespace ControllerFirst.Contexts;

public class AuthContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }
    
    public DbSet<UserCard> UserCards { get; set; }

    public DbSet<UserAddress> UserAddresses { get; set; }

    public AuthContext(DbContextOptions<AuthContext> ops): base(ops)
    {
        
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AuthContext).Assembly); 
    }
}