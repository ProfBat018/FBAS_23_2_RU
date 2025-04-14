namespace DataLayer.Data.Models;


public class ProductAttribute
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; }
    
    public ICollection<AttributeValue> AttributeValues { get; set; }
    public ICollection<CategoryAttribute> CategoryAttributes { get; set; }
}