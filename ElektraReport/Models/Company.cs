using ElektraReport.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElektraReport.Models
{
    public class Company 
    {
        public Guid Id { get; set; }
        public string CompanyName { get; set; }
        public string ApiUrl { get; set; }
        public string ApiKey { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public string Name { get; set; }
        public bool? IsDeleted { get; set; }
        public CompanyType CompanyType { get; set; }
        public DateTime? DeleteTime { get; set; }
    }
}
