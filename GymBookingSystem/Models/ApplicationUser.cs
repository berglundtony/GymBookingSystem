using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GymBookingSystem.Models
{
    public class ApplicationUser : IdentityUser
    {
        //Navigation property
        public ICollection<ApplicationUserGymClass> AttendedClasses { get; set;}

      
    }
}
