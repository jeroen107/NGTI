using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NGTI.Models
{
    public class SoloReservation
    {
        [Key]
        public int IdSoloReservation { get; set; }
        public string Name { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Reason { get; set; }
        public int TableId { get; set; }
        public Table Table { get; set; }
    }
}
