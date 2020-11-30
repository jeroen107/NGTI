using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NGTI.Models
{
    public class CalendarNotificationModel
    {
        public string Name { get; set; }
        public IEnumerable<Attendee> Attendees { get; set; }
        public DateTime StartDateTime { get; set; }
        public string CreatorEmail { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
    }

    public class Attendee
    {
        public string AttendeeName { get; set; }
        public string AttendeeEmail { get; set; }
    }
}
