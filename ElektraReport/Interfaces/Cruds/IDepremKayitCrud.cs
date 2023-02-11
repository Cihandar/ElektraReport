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
        Task<ResultJson<DepremKayit>> Update(VM_DepremKayit model);
        Task<List<VM_DepremKayit>> GetAll(Guid DepremKayitId);
        Task<VM_DepremKayit> GetById(Guid Id);

    }
}
