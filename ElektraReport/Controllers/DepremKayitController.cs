using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElektraReport.Interfaces.Cruds;
using Microsoft.AspNetCore.Mvc;
using ElektraReport.Applications.DepremKayits.ViewModels;
namespace ElektraReport.Controllers
{
    public class DepremKayitController : BaseController
    {

        IDepremKayitCrud _DepremKayitCrud;

        public DepremKayitController(IDepremKayitCrud DepremKayitCrud)
        {
            _DepremKayitCrud = DepremKayitCrud;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<JsonResult> GetAll()
        {
            var result = await _DepremKayitCrud.GetAll(CompanyId);
            return Json(result);
        }

        [HttpGet]
        public async Task<IActionResult> Update(Guid Id)
        {
            var result = await _DepremKayitCrud.GetById(Id);
            return PartialView("_FormPartial", result);
        }
        [HttpPost]
        public async Task<IActionResult> Update(VM_DepremKayit model)
        {
            var result = await _DepremKayitCrud.Update(model);
            return Json(result);
        }



    }
}
