using ElektraReport.Interfaces.ElektraApis;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElektraReport.Models.ReportModels;
using ElektraReport.Models;
using ElektraReport.Infrastructures.SignalR;
using Microsoft.AspNetCore.SignalR;
namespace ElektraReport.Controllers
{
    public class PosNewOrderCheckController : Controller
    {

        private readonly IHubContext<NewOrders> _orderHub;

        public PosNewOrderCheckController( IHubContext<NewOrders> orderHub)
        {
            _orderHub = orderHub;
        }
 
        [HttpGet]
        public  IActionResult  NewOrder(Guid Id)
        {
            _orderHub.Clients.All.SendAsync("CompanyId", Id);
            return Accepted();
        }
    }
}
