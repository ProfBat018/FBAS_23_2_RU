using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechCommerce.DTO.Requests;
using TechCommerce.Services.Interfaces;

namespace TechCommerce.Controllers;


[ApiController]
[Route("api/[controller]/[action]")]
[Authorize(Policy = "AdminPolicy")]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpGet("All")]
    public async Task<IActionResult> GetCategoriesAsync()
    {
        var res = await _categoryService.GetCategoriesAsync();
        
        return Ok(res);
    }
    
    [HttpGet("{name}")]
    public async Task<IActionResult> GetCategoryAsync(string name)
    {
        throw new NotImplementedException();
    }
    
    
    [HttpPost("Create")]
    public async Task<IActionResult> CreateCategoryAsync([FromBody] CreateCategoryDTO dto)
    {
        throw new NotImplementedException();
    }
}