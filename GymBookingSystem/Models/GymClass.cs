using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GymBookingSystem.Models
{
    public class GymClass
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime StartTime { get; set; }
        public TimeSpan Duration { get; set; }
        public DateTime EndTime { get { return StartTime + Duration; } }
        public String Description { get; set; }

        //Navigation property
        public ICollection<ApplicationUserGymClass> AttendingMembers { get; set; }
    }
}
