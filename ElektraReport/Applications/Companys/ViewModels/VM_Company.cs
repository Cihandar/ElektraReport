using AutoMapper;
using ElektraReport.Interfaces.Mappings;
using ElektraReport.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElektraReport.Applications.Companys.ViewModels
{
    public class VM_Company : IHaveCustomMapping
    {
        public Guid Id { get; set; }
        public string CompanyName { get; set; }
        public string ApiUrl { get; set; }
        public string ApiKey { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public void CreateMappings(Profile configuration)
        {
            configuration.CreateMap<VM_Company, Company>();
            configuration.CreateMap<Company, VM_Company>();
        }

    }
}
