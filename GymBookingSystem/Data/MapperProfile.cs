using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Bogus;
using GymBookingSystem.Models;
using GymBookingSystem.Models.ViewModels;
using Microsoft.AspNetCore.Http;

namespace GymBookingSystem.Data
{
    public class MapperProfile: Profile
    {
        public MapperProfile()
        {
            CreateMap<GymClass, GymClassViewModel>();
            CreateMap<GymClass, GymClassViewModel>().ReverseMap();

            CreateMap<GymClass, GymClassViewModel>()
            .ForMember(dest => dest.Attending, from => from.MapFrom<AttendingResolver>());    CreateMap<GymClass, GymClassViewModel>()
                .ForMember(dest => dest.Attending, from => from.MapFrom<AttendingResolver>());

            CreateMap<IEnumerable<GymClass>, IndexViewModel>()
                .ForMember(dest => dest.ShowAll, opt => opt.Ignore())
                .ForMember(dest => dest.GymClasses, from => from.MapFrom(g => g.ToList()));

            CreateMap<IEnumerable<GymClassViewModel>, IndexViewModel>()
            .ForMember(dest => dest.GymClasses, from => from.MapFrom(g => g.ToList()));
        }
    }

    public class AttendingResolver: IValueResolver<GymClass, GymClassViewModel, bool>
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        public AttendingResolver(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }
        public bool Resolve(GymClass source, GymClassViewModel destination, bool destMember, ResolutionContext context)
        {
            return source.AttendingMembers is null ? false :
                source.AttendingMembers.Any(a => a.ApplicationUserId == httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
        }
    }
}
