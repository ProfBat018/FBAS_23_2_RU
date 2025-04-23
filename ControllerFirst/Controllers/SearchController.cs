using ControllerFirst.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ControllerFirst.Controllers;


[Authorize(Policy = "AdminPolicy")]
[ApiController]
[Route("api/v1/[controller]")]
public class SearchController : ControllerBase
{
    private readonly IUserElasticService _elasticService;

    public SearchController(IUserElasticService elasticService)
    {
        _elasticService = elasticService;
    }

    [HttpGet("users/{query}")]
    public async Task<IActionResult> SearchUsers(string query)
    {
        var result = await _elasticService.SearchAsync(query);
        return Ok(result);
    }
}