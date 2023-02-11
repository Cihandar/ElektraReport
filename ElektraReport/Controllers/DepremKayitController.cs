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

namespace ElektraReport.Controllers
{
    public class DepremKayitController : BaseController
    {

        IDepremKayitCrud _DepremKayitCrud;
        ICompanyCrud _company;

        public DepremKayitController(IDepremKayitCrud DepremKayitCrud, ICompanyCrud company)
        {
            _DepremKayitCrud = DepremKayitCrud;
            _company = company;
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
        public async Task<IActionResult> Create(Guid Id)
        {
            var result = new VM_DepremKayit();
            result.CikisTarihi = DateTime.Now.AddDays(7);
            result.GirisTarihi = DateTime.Now;
            result.DogumTarihi = DateTime.Now.AddYears(-20);
            return PartialView("_FormPartial", result);
        }
        [HttpPost]
        public async Task<IActionResult> Create(VM_DepremKayit model)
        {
            var company = await _company.GetById(CompanyId);
            model.OtelAdi = company.CompanyName;
            model.CompanyId = CompanyId;
            var result = await _DepremKayitCrud.Add(model);
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

        [HttpPost]
        public async Task<IActionResult> AddWithExcel(IFormFile file)
        {
            if (file == null)
                return BadRequest("Lütfen Dosya Seçiniz");

            string mime = ContentTypeExtension.GetContent(file.FileName);

            if (mime != "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            {
                return BadRequest("Dosya Tipi Uygun Değil");
            }

            var company = await _company.GetById(CompanyId);
            if (company == null)
                return BadRequest("Company Bulunamadı");

            var model = new List<VM_DepremKayit>();
            var _bFile = await file.GetBytes();
            using (var ms = new MemoryStream(_bFile))
            {
                using (var wb = new XLWorkbook(ms))
                {
                    var ws = wb.Worksheet(1);
                    var rows = ws.RangeUsed().RowsUsed().Skip(1);
                    int index = 1;
                    foreach (var r in rows)
                    {
                        try
                        {
                            if (!string.IsNullOrEmpty(r.Cell(1).Value.ToString()) && !string.IsNullOrEmpty(r.Cell(2).Value.ToString()) && !string.IsNullOrEmpty(r.Cell(3).Value.ToString()) && !string.IsNullOrEmpty(r.Cell(4).Value.ToString()) && !string.IsNullOrEmpty(r.Cell(5).Value.ToString())
                                 && !string.IsNullOrEmpty(r.Cell(6).Value.ToString())
                                  && !string.IsNullOrEmpty(r.Cell(7).Value.ToString())
                                   && !string.IsNullOrEmpty(r.Cell(8).Value.ToString())
                                    && !string.IsNullOrEmpty(r.Cell(9).Value.ToString())
                                     && !string.IsNullOrEmpty(r.Cell(10).Value.ToString())
                                      && !string.IsNullOrEmpty(r.Cell(11).Value.ToString())
                                       && !string.IsNullOrEmpty(r.Cell(12).Value.ToString()))
                            {

                                var _model = new VM_DepremKayit()
                                {
                                    CompanyId = CompanyId,
                                    OtelAdi = company.CompanyName,
                                    Odano = r.Cell(3).Value.ToString(),
                                    TcNo = r.Cell(4).Value.ToString(),
                                    Adi = r.Cell(5).Value.ToString().Trim(),
                                    Soyadi = r.Cell(6).Value.ToString(),
                                    GirisTarihi = DateTime.Parse(r.Cell(7).Value.ToString()),
                                    CikisTarihi = DateTime.Parse(r.Cell(8).Value.ToString()),
                                    DogumTarihi = DateTime.Parse(r.Cell(9).Value.ToString()),
                                    GsmNo = r.Cell(10).Value.ToString(),
                                    GeldigiIl = r.Cell(11).Value.ToString(),
                                    FormVar = r.Cell(12).Value.ToString(),

                                };

                                //UpdatePeriodOperationRequestDtoValidator validator = new UpdatePeriodOperationRequestDtoValidator(_periodValidationlocalizer);
                                //var val = validator.Validate(_model);

                                //if (!val.IsValid)
                                //    return BadRequest(val.Errors.FirstOrDefault()?.ErrorMessage + "\n Satır No: " + index.ToString());

                                model.Add(_model);
                            }

                            index += 1;
                        }
                        catch (Exception ex)
                        {
                            return BadRequest("Hata Oluştu,\n Satır No: " + index.ToString());
                        }
                    }
                }

                if (model != null && !model.Any())
                    return BadRequest("Kayit Oluşmadı");

                var res = await _DepremKayitCrud.Add(model);
                if (!res.Success)
                    return BadRequest(res.Message);
                return Json(res.Data);
            }
        }
    }
}
