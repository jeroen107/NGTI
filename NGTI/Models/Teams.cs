using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace NGTI.Models
{
    public class Teams
    {
       

        [Key]
        public string TeamName { get; set; }
        //public TeamMember TeamMember { get; set; }

        public ICollection<TeamMember> TeamMembers { get; set; }

    }




}
