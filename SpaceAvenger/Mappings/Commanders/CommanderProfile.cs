using AutoMapper;
using SpaceAvenger.DAL.Models;
using SpaceAvenger.ViewModels.UserProfile;

namespace SpaceAvenger.Mappings.Commanders
{
    public class CommanderProfile : Profile
    {
        public CommanderProfile()
        {
            CreateMap<Commander, UserProfileVM>()
                .ForMember(c => c.Id, c=> c.Ignore())
                .ForMember(c => c.Number, c => c.Ignore())
                .ForMember(c => c.OnConfirmButtonPressed, c => c.Ignore())
                .ForMember(c => c.OnSelectButtonPressed, c => c.Ignore())
                .ForMember(c => c.UserName, c => c.MapFrom(x => x.Name))
                .ForMember(c => c.MaleFemale, c => c.MapFrom(x => x.MaleFemale))
                .ForMember(c => c.MissionsCount, c => c.MapFrom(x => x.MissionsCount))
                .ForMember(c => c.EnlistedDate, c => c.MapFrom(x => x.CreatedDate))
                .ForMember(c => c.Points, c => c.MapFrom(x => x.Points))
                .ForMember(c => c.Confirmed, c => c.MapFrom(x => x.Confirmed))
                .ReverseMap()
                .ForPath(s => s.Id, opt => opt.Ignore());
        }
    }
}
