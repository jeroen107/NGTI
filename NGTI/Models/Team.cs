using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NGTI.Models
{
    public class Team
    {
        [Key][Required]
        public string TeamName { get; set; }
        
        public int Members { get; set; }

        
    }
    
    public class EditTeam
    {
        public List<Employee> TeamMembers;
        public List<Employee> Users;
    }
}
