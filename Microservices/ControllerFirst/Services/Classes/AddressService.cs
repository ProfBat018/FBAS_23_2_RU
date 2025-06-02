using AutoMapper;
using Bogus.DataSets;
using ControllerFirst.Contexts;
using ControllerFirst.Data.Models;
using ControllerFirst.DTO.Requests;
using ControllerFirst.DTO.Responses;
using ControllerFirst.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ControllerFirst.Services.Classes;

public class AddressService : IAddressService
{
    private readonly AuthContext _context;
    private readonly IMapper _mapper;

    public AddressService(AuthContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<AddressResponse> AddAddressAsync(string userId, CreateAddressRequest request)
    {
        var user = await _context.Users.FindAsync(Guid.Parse(userId));

        var addressCount = _context.UserAddresses.Count(x => x.UserId == Guid.Parse(userId));
        
        if (addressCount >= 5)
        {
            throw new ArgumentException("User already has 5 addresses");
        }
        
        if (user == null)
        {
            throw new ArgumentException("User not found");
        }

        var address = _mapper.Map<UserAddress>(request);

        address.UserId = Guid.Parse(userId);

        _context.UserAddresses.Add(address);

        await _context.SaveChangesAsync();

        return new AddressResponse(address.Id.ToString(), request.country, request.city, request.street, request.zipCode, request.isDefault);
    }

    public async Task<List<AddressResponse>> GetAddressesAsync(string userId)
    {
        var parsedId = Guid.Parse(userId);

        var userAddresses = await _context.UserAddresses.Where(x => x.UserId == parsedId)
            .Select(x => new AddressResponse(x.Id.ToString(), x.Country, x.City, x.Street, x.PostalCode, x.IsPrimary)).ToListAsync();

        
        if (userAddresses == null || userAddresses.Count == 0)
        {
            throw new ArgumentException("No addresses found for this user");
        }

        return userAddresses;
    }

    public async Task<AddressResponse> UpdateAddressAsync(string userId, string addressId, UpdateAddressRequest request)
    {
        var address = await _context.UserAddresses.FindAsync(addressId);
        
        if (address == null)
        {
            throw new ArgumentException("Address not found");
        }
        
        if (address.UserId != Guid.Parse(userId))
        {
            throw new ArgumentException("This address does not belong to this user");
        }
        

        if (request.isDefault)
        {
            var oldPrimaryAddress = await _context.UserAddresses.FirstOrDefaultAsync(x => x.UserId == address.UserId && x.IsPrimary);

            if (oldPrimaryAddress != null)
            {
                oldPrimaryAddress.IsPrimary = false;
                _context.UserAddresses.Update(oldPrimaryAddress);
            }
        }
        
        _mapper.Map<UserAddress, UpdateAddressRequest>(address, request);

        await _context.SaveChangesAsync();
        
        return new AddressResponse(request.addressId, request.country, request.city, request.street, request.zipCode, request.isDefault);
    }

    public async Task<AddressResponse> DeleteAddressAsync(string userId, string addressId)
    {

        var address = await _context.UserAddresses.FindAsync(addressId);

        if (address == null)
        {
            throw new ArgumentException("Address not found");
        }

        if (address.UserId != Guid.Parse(userId))
        {
            throw new ArgumentException("This address does not belong to this user");
        }

        if (address.IsPrimary)
        {
            throw new Exception("Cannot delete primary address");
        }

        _context.UserAddresses.Remove(address);

        await _context.SaveChangesAsync();

        return new AddressResponse(address.Id.ToString(), address.Country, address.City, address.Street, address.PostalCode, address.IsPrimary);
    }
}