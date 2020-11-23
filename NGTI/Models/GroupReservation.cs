﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NGTI.Models
{
    public class GroupReservation
    {
        [Key]
        public int IdGroupReservation { get; set; }
        public string Name { get; set; }
        public string Teamname { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Reason { get; set; }
        public int TableId { get; set; }
        public Table Table { get; set; }
    }
}