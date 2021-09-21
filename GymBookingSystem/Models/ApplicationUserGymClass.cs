using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GymBookingSystem.Models
{
    public class ApplicationUserGymClass
    {
        public int GymClassId { get; set; }
        public string ApplicationUserId { get; set; }

        public GymClass GymClass { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
}
