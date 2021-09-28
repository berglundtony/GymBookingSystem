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
        [Required(ErrorMessage = "Please enter a name of the gymclass!")]
        [MaxLength(55)]
        [MinLength(3)]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = @"{YYYY-MM-DDTHH:MM:SSZ}")]
        public DateTime? StartTime { get; set; }

        [Required]
        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = @"{0:hh\:mm}")]
        public TimeSpan? Duration { get; set; }
        public DateTime EndTime => StartTime.GetValueOrDefault() + Duration.GetValueOrDefault(); 
        [Required(ErrorMessage = "Please enter a description of the gymclass!")]
        [MaxLength(70)]
        [MinLength(3)]
        public String Description { get; set; }

        //Navigation property
        public ICollection<ApplicationUserGymClass> AttendingMembers { get; set; }
    }
}
