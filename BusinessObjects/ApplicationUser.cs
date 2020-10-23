using Microsoft.AspNetCore.Identity;
using System;

namespace BusinessObjects
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
