using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElektraReport.Applications.Auths.ViewModels
{
    public class VM_LoginModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public bool RemenberMe { get; set; }
    }
}
