using ElektraReport.Models.ApiModels;
using ElektraReport.Models.App;
using ElektraReport.Models.ReportModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElektraReport.Interfaces.ElektraApis
{
    public interface IApiRequest
    {
         Task<ApiResults<PosForecast>> GetPosForecast(Guid CompanyId, DateTime date,DateTime date2);
        Task<ApiResults<PosSaleReport>> GetPosSaleReport(Guid CompanyId, DateTime date, DateTime date2);
        Task<ApiResults<PosDicountReport>> GetPosDiscountReport(Guid CompanyId, DateTime date, DateTime date2);
        Task<ApiResults<PosNotPayReport>> GetPosNotPayReport(Guid CompanyId, DateTime date, DateTime date2);
        Task<ApiResults<PosCreditReport>> GetPosCreditReport(Guid CompanyId, DateTime date, DateTime date2);
        Task<ApiResults<PosCancelReport>> GetPosCancelReport(Guid CompanyId, DateTime date, DateTime date2);
        Task<ApiResults<PosNotPayDetailReport>> GetPosNotPayDetailReport(Guid CompanyId, DateTime date, DateTime date2);
        Task<ApiResults<PosProductSalesReport>> GetPosProductSalesReport(Guid CompanyId, DateTime date, DateTime date2);
        Task<ApiResults<PosChartReport>> GetPosChartReport(Guid CompanyId, DateTime date, DateTime date2);
        Task<ApiResults<PosOpenTables>> GetPosOpenTables(Guid CompanyId);
        Task<ApiResults<PosOpenTablesDetails>> GetPosOpenTablesDetails(Guid CompanyId, int Id);
        Task<ApiResults<PosOrders>> GetPosOrders(Guid CompanyId);
    }
}
