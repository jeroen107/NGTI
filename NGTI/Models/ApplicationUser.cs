using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NGTI.Models
{
    public class ApplicationUser : IdentityUser
    {
       
        public bool BHV { get; set; }
        public bool Admin { get; set; }
    }
}
