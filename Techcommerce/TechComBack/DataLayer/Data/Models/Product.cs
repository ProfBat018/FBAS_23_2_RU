namespace DataLayer.Data.Models;


public class Product
{
    public Guid Id { get; set; } = Guid.NewGuid();
    
    public string ProductName { get; set; }
    
    public string Description { get; set; }

    public string ImagePath { get; set; }
    
    public decimal Price { get; set; }

    public ICollection<Category> Categories { get; set; }
    public ICollection<Warehouse> Warehouses { get; set; }
    public ICollection<ProductCategory> ProductCategories { get; set; }
    
}