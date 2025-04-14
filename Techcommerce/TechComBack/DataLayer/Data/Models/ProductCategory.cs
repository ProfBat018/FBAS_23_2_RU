namespace DataLayer.Data.Models;


    public class ProductCategory
    {
        public string CategoryRef { get; set; }
        public Guid ProductRef { get; set; }
        
        public Category Category { get; set; }
        public Product Product { get; set; }
    }