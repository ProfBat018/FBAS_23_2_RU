namespace DataLayer.Data.Models;


public class CategoryAttribute
{
    public string CategoryRef { get; set; }
    public Guid AttributesRef { get; set; }
    
    public Category Category { get; set; }
    public ProductAttribute ProductAttribute { get; set; }
}