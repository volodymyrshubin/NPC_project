using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PMS.Models.Context
{
    public class ApplicationUser : IdentityUser<int>
    {
        public string Name { get; set; }
        public string UserType { get; set; }
    }
    public class ApplicationRole : IdentityRole<int>
    {
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public string IPAddress { get; set; }
    }
}
