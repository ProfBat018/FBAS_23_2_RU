using AutoMapper;
using ControllerFirst.Data.Models;
using ControllerFirst.DTO.Requests;

namespace ControllerFirst.Data.Mapping;

public class UserAddressProfile : Profile
{
    public UserAddressProfile()
    {
        CreateMap<CreateAddressRequest, UserAddress>()
            .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.country))
            .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.city))
            .ForMember(dest => dest.Street, opt => opt.MapFrom(src => src.street))
            .ForMember(dest => dest.PostalCode, opt => opt.MapFrom(src => src.zipCode))
            .ForMember(dest => dest.IsPrimary, opt => opt.MapFrom(src => src.isDefault))
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.User, opt => opt.Ignore())
            .ForMember(dest => dest.UserId, opt => opt.Ignore());

        CreateMap<UpdateAddressRequest, UserAddress>()
            .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.country))
            .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.city))
            .ForMember(dest => dest.Street, opt => opt.MapFrom(src => src.street))
            .ForMember(dest => dest.PostalCode, opt => opt.MapFrom(src => src.zipCode))
            .ForMember(dest => dest.IsPrimary, opt => opt.MapFrom(src => src.isDefault))
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.User, opt => opt.Ignore())
            .ForMember(dest => dest.UserId, opt => opt.Ignore());
    }
}
