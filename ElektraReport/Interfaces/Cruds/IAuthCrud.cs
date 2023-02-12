using ElektraReport.Applications.Auths.ViewModels;
using ElektraReport.Models;
using ElektraReport.Models.ResultModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElektraReport.Interfaces.Cruds
{
    public interface IAuthCrud
    {
        Task<ResultJson<AppUser>> Register(VM_AuthRegister model);
        Task<ResultJson<AppUser>> Login(VM_LoginModel model);
        Task<ResultJson<AppUser>> SetActiveUser(string email);
        Task<List<VM_PassiveUsers>> GetAllPassiveUsers();
    }
}
