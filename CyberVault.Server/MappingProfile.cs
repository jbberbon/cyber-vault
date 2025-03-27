using AutoMapper;
using CyberVault.Server.DTO.User;
using CyberVault.Server.Models;

namespace CyberVault;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<UserForRegistrationDto, User>()
            .ForMember(u => u.UserName, opt => opt.MapFrom(x => x.Email));
    }
}