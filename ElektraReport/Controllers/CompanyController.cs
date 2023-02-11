using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElektraReport.Interfaces.Cruds;
using Microsoft.AspNetCore.Mvc;
using ElektraReport.Applications.Companys.ViewModels;
namespace ElektraReport.Controllers
{
    public class CompanyController : BaseController
    {

        ICompanyCrud _companyCrud;

        public CompanyController(ICompanyCrud companyCrud)
        {
            _companyCrud = companyCrud;
        }

        public IActionResult Index()
        {
            if (!Admin) return Redirect("/Auth/Logout");
            return View();
        }

        [HttpGet]
        public async Task<JsonResult> GetAll()
        {
          
            var result = await _companyCrud.GetAll(CompanyId);
            return Json(result);
        }

        [HttpGet]
        public async Task<IActionResult> Update(Guid Id)
        {
            var result = await _companyCrud.GetById(Id);
            return PartialView("_FormPartial", result);
        }
        [HttpPost]
        public async Task<IActionResult> Update(VM_Company model)
        {
            var result = await _companyCrud.Update(model);
            return Json(result);
        }



    }
}
