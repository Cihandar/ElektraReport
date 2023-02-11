using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElektraReport.Models
{
    public class AppUser : IdentityUser
    {
        public string Avatar { get; set; }
        public byte Status { get; set; }
        public Guid CompanyId { get; set; }
        public string FullName { get; set; }
        public bool Admin { get; set; }
    }
}
