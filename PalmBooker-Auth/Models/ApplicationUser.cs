using Microsoft.AspNetCore.Identity;
using System;

namespace EBookkeepingAuth.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {

        public ApplicationUser()
        {
        }

        public ApplicationUser(string userName) : base(userName)
        {
        }

        
        public DateTime DateCreated { get; set; }
       
    }
}
