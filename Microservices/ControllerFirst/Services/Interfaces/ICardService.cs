using System.Security.Claims;
using ControllerFirst.DTO.Requests;
using ControllerFirst.DTO.Responses;

namespace ControllerFirst.Services.Interfaces;

public interface ICardService
{
    public Task<CardResponse> CreateCardAsync(ClaimsPrincipal userClaimsPrincipal, CreateCardRequest request);
    public Task<CardResponse> DeleteCardAsync(ClaimsPrincipal userClaimsPrincipal,DeleteCardRequest request);
    public Task<CardResponse> SetAsPrimaryAsync(ClaimsPrincipal useClaimsPrincipal, SetAsPrimaryRequest request);
    
    public Task<List<CardResponse>> GetCardsAsync(ClaimsPrincipal userClaimsPrincipal);
    
    
    
}