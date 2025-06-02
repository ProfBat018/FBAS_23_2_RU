using AutoMapper;
using ControllerFirst.Data.Models;
using ControllerFirst.DTO.Requests;

namespace ControllerFirst.Data.Mapping;

public class UserCardProfile : Profile
{
    public UserCardProfile()
    {
        CreateMap<CreateCardRequest, UserCard>()
            .ForMember(dest => dest.CardHolder, opt => opt.MapFrom(src => src.cardHolderName))
            .ForMember(dest => dest.IsDefault, opt => opt.MapFrom(src => src.isDefault))
            .ForMember(dest => dest.ExpirationMonth, opt => opt.MapFrom(src => ParseMonth(src.expirationDate)))
            .ForMember(dest => dest.ExpirationYear, opt => opt.MapFrom(src => ParseYear(src.expirationDate)))
            .ForMember(dest => dest.CardNumberEncrypted, opt => opt.Ignore())
            .ForMember(dest => dest.CVVEncrypted, opt => opt.Ignore())
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.User, opt => opt.Ignore());

        CreateMap<UpdateCardRequest, UserCard>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.Parse(src.cardId)))
            .ForMember(dest => dest.CardNumberEncrypted, opt => opt.Ignore())
            .ForMember(dest => dest.CVVEncrypted, opt => opt.Ignore())
            .ForMember(dest => dest.ExpirationMonth, opt => opt.MapFrom(src => ParseMonth(src.expirationDate)))
            .ForMember(dest => dest.ExpirationYear, opt => opt.MapFrom(src => ParseYear(src.expirationDate)))
            .ForMember(dest => dest.IsDefault, opt => opt.MapFrom(src => src.isDefault))
            .ForMember(dest => dest.CardHolder, opt => opt.MapFrom(src => src.holder));
    }

    private static int ParseMonth(string expirationDate)
    {
        var parts = expirationDate.Split('/');
        return int.Parse(parts[0]);
    }

    private static int ParseYear(string expirationDate)
    {
        var parts = expirationDate.Split('/');
        var year = parts[1];
        return year.Length == 2 ? 2000 + int.Parse(year) : int.Parse(year);
    }
}