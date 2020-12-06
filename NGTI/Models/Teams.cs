using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NGTI.Models
{
    public class Teams
    {
        [Key]
        public string TeamName { get; set; }
        public ICollection<GroupReservation> GroupReservations { get; set; } = new List<GroupReservation>();
        
       
    }


}
