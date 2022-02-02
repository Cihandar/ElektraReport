using ElektraReport.Interfaces.ElektraApis;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElektraReport.Models.ReportModels;
using ElektraReport.Models;

namespace ElektraReport.Controllers
{
    public class PosOpenTablesController : BaseController
    {
        IApiRequest _apiRequest;
        public PosOpenTablesController(IApiRequest apiRequest)
        {
            _apiRequest = apiRequest;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> GetOpenTables(RequestModel model)
        {
            var result = await _apiRequest.GetPosOpenTables(CompanyId);
            return Json(result);
        }
         
        public async Task<IActionResult> GetOpenTablesDetails(int Id)
        {
            var result = await _apiRequest.GetPosOpenTablesDetails(CompanyId,Id);
            return PartialView("_FormPartial",result.Data);
        }
    }
}
