

using DataLayer.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataLayer.Data.FluentConfigs;


public class WarehouseConfig: IEntityTypeConfiguration<Warehouse>
{
    public void Configure(EntityTypeBuilder<Warehouse> entity)
    {
        entity.HasKey(w => w.ProductId);
        entity.Property(w => w.Count).IsRequired();

        entity.HasOne(w => w.Product)
            .WithMany(p => p.Warehouses)
            .HasForeignKey(w => w.ProductId);
    }
}

