using System.Collections;
using DataLayer.Data.Models;
using Microsoft.EntityFrameworkCore;
using TechCommerce.Data.Contexts;
using TechCommerce.DTO.Requests;
using TechCommerce.DTO.Response;
using TechCommerce.Services.Interfaces;


namespace TechCommerce.Services.Implementations;

public class CategoryService : ICategoryService
{
    private readonly TechContext _context;

    public CategoryService(TechContext context)
    {
        _context = context;
    }

    public async Task<CategoryResponseDTO> CreateCategoryAsync(CreateCategoryDTO dto)
    {
        var newCategory = new Category
        {
            CategoryName = dto.name,
            CategoryNameRef = dto.parentName
        };

        _context.Categories.Add(newCategory);

        await _context.SaveChangesAsync();

        return new CategoryResponseDTO(dto.name, dto.parentName);
    }

    public async Task<CategoryResponseDTO> GetCategoryAsync(string name)
    {
        var category = await _context.Categories.FindAsync(name);
        if (category == null)
        {
            throw new Exception("There is no category with this name");
        }

        return new CategoryResponseDTO(category.CategoryName, category.CategoryNameRef);
    }



    public async Task<IEnumerable<CategoryResponseDTO>> GetCategoriesAsync(int page = 1, int pageSize = 20)
    {
        var categories = _context.Categories
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(c => new CategoryResponseDTO(c.CategoryName, c.CategoryNameRef)).AsNoTracking();

        return await categories.ToListAsync();
    }
}