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
    public class PosNotPayReportController : BaseController
    {
        IApiRequest _apiRequest;
        public PosNotPayReportController(IApiRequest apiRequest)
        {
            _apiRequest = apiRequest;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GetReport(RequestModel model)
        {
            var result = await _apiRequest.GetPosNotPayReport(CompanyId, model.date , model.date2);
            return Json(result);
        }
    }
}
