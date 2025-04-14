namespace DataLayer.Data.Models;


public class Warehouse
{
    public Guid ProductId { get; set; }
    public Product Product { get; set; }
    public int Count { get; set; } = 0;



}