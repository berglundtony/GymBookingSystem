using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GymBookingSystem.Models.ViewModels
{
    public class IndexViewModel
    {
        public IEnumerable<GymClassViewModel> GymClasses { get; set; }
        public bool ShowAll { get; set; }
        public bool History { get; set; }
        public bool Booked { get; set; }
    }
}
