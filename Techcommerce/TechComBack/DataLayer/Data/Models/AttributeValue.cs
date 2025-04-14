namespace DataLayer.Data.Models;

public class AttributeValue
{
    public Guid AttributeId { get; set; }
    public ProductAttribute ProductAttribute { get; set; }

    public string Value { get; set; }

    public string CategoryRef { get; set; }
    public Category Category { get; set; }
}