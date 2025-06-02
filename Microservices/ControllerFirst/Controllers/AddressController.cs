using System.Security.Claims;
using ControllerFirst.DTO.Requests;
using ControllerFirst.DTO.Responses;
using ControllerFirst.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ControllerFirst.Controllers;

[ApiController]
[Authorize]
[Route("api/v1/[controller]")]
public class AddressController : ControllerBase
{
    private readonly IAddressService _addressService;

    public AddressController(IAddressService addressService)
    {
        _addressService = addressService;
    }

    [HttpPost("Add")]
    public async Task<IActionResult> AddAddressAsync(CreateAddressRequest request)
    {
        var userId = User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
        
        var res = await _addressService.AddAddressAsync(userId, request);
        
        return Ok(Result<AddressResponse>.Success(res, "Address added successfully"));
    }
    
    [HttpGet("All")]
    public async Task<IActionResult> GetAddressesAsync()
    {
        var userId = User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;

        var res = await _addressService.GetAddressesAsync(userId);
        
        return Ok(Result<List<AddressResponse>>.Success(res, "Addresses retrieved successfully"));
        
    }
    
    [HttpDelete("Delete")]
    public async Task<IActionResult> DeleteAddressAsync(DeleteAddressRequest request)
    {
        var userId = User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;

        var res = await _addressService.DeleteAddressAsync(userId, request.addressId);
        
        return Ok(Result<AddressResponse>.Success(res, "Address deleted successfully"));
    }
    
    [HttpPost("Update")]
    public async Task<IActionResult> UpdateAddressAsync(UpdateAddressRequest request)
    {
        var userId = User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;

        var res = await _addressService.UpdateAddressAsync(userId, request.addressId, request);
        
        return Ok(Result<AddressResponse>.Success(res, "Address updated successfully"));
    }
}
