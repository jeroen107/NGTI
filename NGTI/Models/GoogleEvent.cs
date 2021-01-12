using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NGTI.Models
{
    public class GoogleEvent
    {
        public string Summary { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
        public GoogleEventDateTime Start { get; set; }
        public GoogleEventDateTime End { get; set; }
        public string[] Recurrence { get; set; }
        public GoogleEventAttendee[] Attendees { get; set; }

    }
}
