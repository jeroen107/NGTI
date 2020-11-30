using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NGTI.Models
{
    public class Table
    {
        [Key]
        public int TableId { get; set; }
        // verbinding tussen groepreservatie en tafels
        public ICollection<GroupReservation> GroupReservations { get; set; } = new List<GroupReservation>();
        public ICollection<SoloReservation> SoloReservations { get; set; } = new List<SoloReservation>();
    }
}
