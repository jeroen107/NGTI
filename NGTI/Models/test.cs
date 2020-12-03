using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NGTI.Models
{
    public class test
    {
        [Key]
        public int id { get; set; }
        public string name { get; set; }
    }
}
