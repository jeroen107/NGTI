using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace NGTI.Models
{
    public class ApplicationUser : IdentityUser
    {
      
      
        public bool BHV { get; set; }
        public bool Admin { get; set; }

        public string TokenKey { get; set; }

        public string TokenValue { get; set; }


        public ICollection<TeamMember> TeamMembers { get; set; }

        /*public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser, int> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);

            // Add custom user claims here
            userIdentity.AddClaim(new Claim(Admin.ToString(), ClaimValueTypes.Boolean));

            return userIdentity;
        }*/
        


    }
}
