using ControllerFirst.DTO.Requests;
using ControllerFirst.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ControllerFirst.Controllers;

[Authorize]
[ApiController]
[Route("api/v1/[controller]")]
public class CardController : ControllerBase
{
    private readonly ICardService _cardService;

    public CardController(ICardService cardService)
    {
        _cardService = cardService;
    }

    
    [HttpGet("All")]
    public async Task<IActionResult> GetCardsAsync()
    {
        throw new NotImplementedException();
    }


    [HttpPost("Add")]
    public async Task<IActionResult> CreateCardAsync([FromBody] CreateCardRequest request)
    {
        throw new NotImplementedException();
    }
    
    [HttpDelete]
    public async Task<IActionResult> DeleteCardAsync([FromBody] DeleteCardRequest request)
    {
        throw new NotImplementedException();
    }
    
    [HttpPost("Update")]
    public async Task<IActionResult> UpdateCardAsync([FromBody] UpdateCardRequest request)
    {
        throw new NotImplementedException();
    }
    
}