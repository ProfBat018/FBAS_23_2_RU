using ControllerFirst.DTO.Requests;
using ControllerFirst.DTO.Responses;

namespace ControllerFirst.Services.Interfaces;

public interface IAddressService
{
    public Task<AddressResponse> AddAddressAsync(string userId, CreateAddressRequest request);
    public Task<List<AddressResponse>> GetAddressesAsync(string userId);

    public Task<AddressResponse> UpdateAddressAsync(string userId, string addressId, UpdateAddressRequest request);
    public Task<AddressResponse> DeleteAddressAsync(string userId, string addressId);
}