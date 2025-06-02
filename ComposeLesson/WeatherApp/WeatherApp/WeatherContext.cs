using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace WeatherApp;

public class WeatherContext : DbContext
{
    public DbSet<WeatherForecast> Forecast { get; set; }
    public WeatherContext(DbContextOptions<WeatherContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var entityType = modelBuilder.Entity<WeatherForecast>();

        entityType.HasKey(e => e.Id);
        entityType.Property(e => e.Id)
            .ValueGeneratedOnAdd()
            .HasColumnName("Id");

    }
}