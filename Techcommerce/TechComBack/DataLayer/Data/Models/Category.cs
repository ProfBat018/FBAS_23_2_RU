namespace DataLayer.Data.Models;


public class Category
{
    public string CategoryName { get; set; }

    public string CategoryNameRef { get; set; }
    public Category ParentCategory { get; set; }
    
    public ICollection<Category> SubCategories { get; set; }
    public ICollection<AttributeValue> AttributeValues { get; set; }
    public ICollection<ProductCategory> ProductCategories { get; set; }
    public ICollection<CategoryAttribute> CategoryAttributes { get; set; }
    
}