using AutoMapper;
using FourTails.Core.DomainModels;
using FourTails.DTOs.Payload.User;

namespace FourTails.Api.Configurations.Mappings;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, UserUpdateDetailsDTO>()
            .ForMember(dest => dest.Age, opt => opt.MapFrom(src => src.Age))
            .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
            .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
            .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName));
    }
}