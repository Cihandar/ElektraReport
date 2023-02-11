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
    }
}
