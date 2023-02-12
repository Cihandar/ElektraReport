using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElektraReport.Applications.Companys.ViewModels;
namespace ElektraReport.Applications.Auths.ViewModels
{
    public class VM_AuthRegister
    {
        public VM_Company Company { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool ServiceAgreement { get; set; }
        public bool MessageDelivery { get; set; }
        public bool Kvkk { get; set; }
        public string FullName { get; set; }
        public string ClientIp { get; set; }
    }
}
