using DataLayer.Data.Models;
using TechCommerce.DTO.Requests;
using TechCommerce.DTO.Response;


namespace TechCommerce.Services.Interfaces;

public interface ICategoryService
{
    public Task<CategoryResponseDTO> CreateCategoryAsync(CreateCategoryDTO dto);
    public Task<CategoryResponseDTO> GetCategoryAsync(string name);
    public Task<IEnumerable<CategoryResponseDTO>> GetCategoriesAsync(int page = 1, int pageSize = 20);
}