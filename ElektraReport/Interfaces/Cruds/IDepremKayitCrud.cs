using ElektraReport.Applications.DepremKayits.ViewModels;
using ElektraReport.Models;
using ElektraReport.Models.ResultModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ElektraReport.Interfaces.Cruds
{
    public interface IDepremKayitCrud
    {
        Task<ResultJson<DepremKayit>> Add(VM_DepremKayit model);
        Task<ResultJson<DepremKayit>> Add(List<VM_DepremKayit> model);
        Task<ResultJson<DepremKayit>> Update(VM_DepremKayit model);
        Task<List<VM_DepremKayit>> GetAll(Guid DepremKayitId);
        Task<List<VM_DepremKayit>> GetAllOtel(Guid companyId, string adsoyad, string tcno);
        Task<List<VM_DepremKayit>> GetAllOtelBlackList(Guid companyId, string adsoyad, string tcno);
        Task<VM_DepremKayit> GetById(Guid Id);
        Task<bool> CheckOut(Guid Id, string ip);
        Task<bool> Delete(Guid Id, string ip);

        Task<VM_Dashboard> Dashboards();
        Task<List<VM_DepremKayitDashboard>> Dashboards2(Guid CompanyId);
    }
}
