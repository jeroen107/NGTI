using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace NGTI.Models
{
    public class TeamMember
    {
       
       public string TeamName { get; set; }
       public Teams Teams { get; set; }
        
       public string UserId { get; set; }
       [ForeignKey("UserId")]
       public ApplicationUser ApplicationUser { get; set; }
        

        

    }
}
