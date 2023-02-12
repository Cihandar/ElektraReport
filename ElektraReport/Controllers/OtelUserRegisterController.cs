using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElektraReport.Interfaces.Cruds;
using Microsoft.AspNetCore.Mvc;
using ElektraReport.Applications.DepremKayits.ViewModels;
using Microsoft.AspNetCore.Http;
using ElektraReport.Components;
using System.IO;
using ClosedXML.Excel;
using ElektraReport.Models;

namespace ElektraReport.Controllers
{
    public class OtelUserRegisterController : BaseController
    {

        IAuthCrud _authCrud;

        public OtelUserRegisterController(IAuthCrud authCrud)
        {
            _authCrud = authCrud;
        }

        public IActionResult Index()
        {
            if (!Admin) return Redirect("/Auth/Logout");
            return View();
        }

        [HttpGet]
        public async Task<JsonResult> GetAll()
        {      
            var result = await _authCrud.GetAllPassiveUsers();
            return Json(result);
        }


        //[HttpGet]
        //public async Task<IActionResult> Update(Guid Id)
        //{
        //    var result = await _authCrud.GetById(Id);
        //    return PartialView("_FormPartial", result);
        //}
        [HttpPost]
        public async Task<IActionResult> SetActiveUser(string email)
        {
            var result = await _authCrud.SetActiveUser(email);
            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> SetDisable(string email)
        {
            var result = await _authCrud.SetDisableUser(email);
            return Json(result);
        }
    }
}
