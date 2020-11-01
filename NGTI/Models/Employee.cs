using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NGTI.Models
{
    public class Employee
    {
        [Key]
        public string Email { get; set; }
        public string Name { get; set; }
        public bool BHV { get; set; }
        public bool Admin { get; set; }
    }
}
