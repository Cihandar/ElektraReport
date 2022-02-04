using ElektraReport.Interfaces.ElektraApis;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElektraReport.Models.ReportModels;
using ElektraReport.Models;
using ElektraReport.Infrastructures.SignalR;

namespace ElektraReport.Controllers
{
    public class PosOrderController : BaseController
    {
        IApiRequest _apiRequest;
        NewOrders _signalRnewOrders;
       
        public PosOrderController(IApiRequest apiRequest,NewOrders signalRnewOrders)
        {
            _apiRequest = apiRequest;
            _signalRnewOrders = signalRnewOrders;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> GetPosOrders()
        {
            var result = await _apiRequest.GetPosOrders(CompanyId);
            return Json(result);
        }

        [HttpGet]
        public async Task<IActionResult> NewOrder(Guid Id)
        {
            _signalRnewOrders.SendNewOrders(Id);

            return null;
        }
    }
}
