using ElektraReport.Applications.Companys.ViewModels;
using ElektraReport.Models;
using ElektraReport.Models.ResultModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ElektraReport.Interfaces.Cruds
{
    public interface ICompanyCrud
    {
        Task<ResultJson<Company>> Add(VM_Company model);
        Task<ResultJson<Company>> Update(VM_Company model);
        Task<List<VM_Company>> GetAll(Guid CompanyId);
        Task<VM_Company> GetById(Guid Id);

    }
}
