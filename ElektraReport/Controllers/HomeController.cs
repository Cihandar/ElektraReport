using ElektraReport.Interfaces.ElektraApis;
using ElektraReport.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElektraReport.Controllers
{

    public class HomeController : BaseController
    {
        IApiRequest _apiRequest;
        public HomeController(IApiRequest apiRequest)
        {
            _apiRequest = apiRequest;
        }


        public IActionResult Index()
        {
            ViewBag.CompanyId = CompanyId;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GetReport(RequestModel model)
        {
            var result = await _apiRequest.GetPosForecast(CompanyId, model.date, model.date2);
 
            return PartialView("Default", result);
        }

        [HttpPost]
        public async Task<IActionResult> GetChartReport()
        {
            var result = await _apiRequest.GetPosChartReport(CompanyId, DateTime.Now.AddDays(-14), DateTime.Now.AddDays(-7));

            return Json(result.Data);
        }

    }
}
