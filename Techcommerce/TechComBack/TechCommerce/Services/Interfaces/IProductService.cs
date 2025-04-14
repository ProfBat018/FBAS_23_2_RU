using System.Linq.Expressions;
using DataLayer.Data.Models;
using TechCommerce.DTO.Response;

namespace TechCommerce.Services.Interfaces;

public interface IProductService
{
    public Task<ProductResponseDTO> CreateProductAsync(ProductResponseDTO dto);
    public Task<IEnumerable<ProductResponseDTO>> GetProductAsync(string name);
    public Task<IEnumerable<ProductResponseDTO>> GetProductsAsync(int page = 1, int pageSize = 20);
}

