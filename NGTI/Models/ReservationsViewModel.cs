using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Threading.Tasks;

namespace NGTI.Models
{
    public class ReservationsViewModel
    {
        public List<SoloReservation> soloList {get; set;}
        public List<GroupReservation> groupList { get; set; }
    }
}
