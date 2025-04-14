using System.Linq.Expressions;
using DataLayer.Data.Models;
using Microsoft.EntityFrameworkCore;
using TechCommerce.Data.Contexts;
using TechCommerce.DTO.Response;
using TechCommerce.Services.Interfaces;


namespace TechCommerce.Services.Implementations;

public class ProductService : IProductService
{
    private readonly TechContext _context;

    public ProductService(TechContext context)
    {
        _context = context;
    }

    public async Task<ProductResponseDTO> CreateProductAsync(ProductResponseDTO dto)
    {
        var newProduct = new Product
        {
            ProductName = dto.productName,
            Description = dto.productDescription,
            Price = dto.price,
            ImagePath = dto.imagePath
        };
        
        await _context.Products.AddAsync(newProduct);
        
        await _context.SaveChangesAsync();
        
        return new ProductResponseDTO(dto.productName, dto.productDescription, dto.price, dto.imagePath);
    }

    public async Task<IEnumerable<ProductResponseDTO>> GetProductAsync(string name)
    {
        var product = await _context.Products.Where(p => p.ProductName.ToLower() == name.ToLower())
            .Select(p => new ProductResponseDTO(p.ProductName, p.Description, p.Price, p.ImagePath)).ToListAsync();

        return product;
    }

    public async Task<IEnumerable<ProductResponseDTO>> GetProductsAsync(int page = 1, int pageSize = 20)
    {
        var products = _context.Products
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(p => new ProductResponseDTO(p.ProductName, p.Description, p.Price, p.ImagePath)).AsNoTracking();

        return await products.ToListAsync();
    }
}