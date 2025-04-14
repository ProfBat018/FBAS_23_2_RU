using DataLayer.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace TechCommerce.Data.Contexts;

public class TechContext : DbContext
{
    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<ProductAttribute> Attributes { get; set; }
    public DbSet<AttributeValue> AttributeValues { get; set; }
    public DbSet<CategoryAttribute> CategoryAttributes { get; set; }
    public DbSet<ProductCategory> ProductCategories { get; set; }
    public DbSet<Warehouse> Warehouses { get; set; }

    public TechContext(DbContextOptions<TechContext> ops) : base(ops)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TechContext).Assembly); 
    }

    public DbContextOptions Options { get; }
    public bool UseUserDefinedTransaction { get; set; }
}