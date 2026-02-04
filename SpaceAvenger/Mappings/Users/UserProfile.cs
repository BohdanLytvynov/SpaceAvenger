using AutoMapper;
using SpaceAvenger.DAL.Models;
using SpaceAvenger.ViewModels.UserProfile;

namespace SpaceAvenger.Mappings.Users
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserProfileVM>()
                .ForMember(c => c.Id, c => c.MapFrom(x => x.Id))
                .ForMember(c => c.Number, c => c.Ignore())
                .ForMember(c => c.OnConfirmButtonPressed, c => c.Ignore())
                .ForMember(c => c.OnSelectButtonPressed, c => c.Ignore())
                .ForMember(c => c.UserName, c => c.MapFrom(x => x.ProfileName))
                .ReverseMap();
        }
    }
}
