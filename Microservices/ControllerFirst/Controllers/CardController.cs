using ControllerFirst.DTO.Requests;
using ControllerFirst.DTO.Responses;
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
        var res = await _cardService.GetCardsAsync(User);
        
        return Ok(Result<List<CardResponse>>.Success(res, "Cards retrieved successfully"));
    }


    [HttpPost("Add")]
    public async Task<IActionResult> CreateCardAsync([FromBody] CreateCardRequest request)
    {
        var res = await _cardService.CreateCardAsync(User, request);
        
        return Ok(Result<CardResponse>.Success(res, "Card created successfully"));
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteCardAsync([FromBody] DeleteCardRequest request)
    {
        var res = await _cardService.DeleteCardAsync(User, request);
        
        return Ok(Result<CardResponse>.Success(res, "Card deleted successfully"));
    }

    [HttpPost("SetAsPrimary")]
    public async Task<IActionResult> SetAsPrimaryAsync([FromBody] SetAsPrimaryRequest request)
    {
        var res = await _cardService.SetAsPrimaryAsync(User, request);
        
        return Ok(Result<CardResponse>.Success(res, "Card set as primary successfully"));
    }
   
}