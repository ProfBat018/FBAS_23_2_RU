using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ControllerFirst.Controllers;

[ApiController]
[Authorize]
[Route("api/v1/[controller]")]
public class AddressController : ControllerBase
{
    [HttpPost("Add")]
    public async Task<IActionResult> AddAddressAsync()
    {
        throw new NotImplementedException();
    }
    
    [HttpGet("All")]
    public async Task<IActionResult> GetAddressesAsync()
    {
        throw new NotImplementedException();
    }
    
    [HttpDelete("Delete")]
    public async Task<IActionResult> DeleteAddressAsync()
    {
        throw new NotImplementedException();
    }
    
    [HttpPost("Update")]
    public async Task<IActionResult> UpdateAddressAsync()
    {
        throw new NotImplementedException();
    }
}