using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Bogus;
using GymBookingSystem.Models;
using GymBookingSystem.Models.ViewModels;

namespace GymBookingSystem.Data
{
    public class MapperProfile: Profile
    {
        public MapperProfile()
        {
            var faker = new Faker();
            CreateMap<GymClass, GymClassViewModel>();
            CreateMap<GymClass, GymClassViewModel>()
            .ForMember(dest => dest.Booked,
            from => from.MapFrom(b => b.AttendingMembers.Select(a => a.ApplicationUserId)));
        }
    }
}
