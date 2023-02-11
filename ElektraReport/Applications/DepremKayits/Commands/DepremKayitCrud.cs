using AutoMapper;
using ElektraReport.Applications.Context;
using ElektraReport.Applications.DepremKayits.ViewModels;
using ElektraReport.Interfaces.Cruds;
using ElektraReport.Models;
using ElektraReport.Models.ResultModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace ElektraReport.Applications.DepremKayits.Commands
{
    public class DepremKayitCrud : IDepremKayitCrud
    {
        public DatabaseContext _context;
        public IMapper _mapper;

        public DepremKayitCrud(DatabaseContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ResultJson<DepremKayit>> Add(VM_DepremKayit model)
        {
            try
            {
                var DepremKayit = _mapper.Map<DepremKayit>(model);
                _context.DepremKayits.Add(DepremKayit);
                _context.SaveChanges();
                return new ResultJson<DepremKayit> { Success = true, Message = "Rezervasyon kayıt edildi.", Data = DepremKayit };
            }
            catch (Exception ex)
            {
                return new ResultJson<DepremKayit> { Success = false, Message = "Hata : " + ex.Message };
            }

        }

        public async Task<ResultJson<DepremKayit>> Add(List<VM_DepremKayit> model)
        {
            try
            {
                List<ResultJson<DepremKayit>> response = new List<ResultJson<DepremKayit>>();
                foreach (var item in model)
                {
                    var res = await Add(item);
                    response.Add(res);
                    if (!res.Success)
                        return res;
                }

                return response.FirstOrDefault() ?? new ResultJson<DepremKayit>();
            }
            catch (Exception ex)
            {
                return new ResultJson<DepremKayit> { Success = false, Message = "Hata : " + ex.Message };
            }

        }

        public async Task<ResultJson<DepremKayit>> Update(VM_DepremKayit model)
        {
            try
            {
                var depremKayit = _context.DepremKayits.FirstOrDefault(x => x.Id == model.Id);
                depremKayit.Adi = model.Adi;
                depremKayit.CikisTarihi = model.CikisTarihi;
                depremKayit.Cinsiyet = model.Cinsiyet;
                depremKayit.DogumTarihi = model.DogumTarihi;
                depremKayit.FormVar = model.FormVar;
                depremKayit.GeldigiIl = model.GeldigiIl;
                depremKayit.GirisTarihi = model.GirisTarihi;
                depremKayit.GsmNo = model.GsmNo;
                depremKayit.Odano = model.Odano;
                depremKayit.RezervasyonNo = model.RezervasyonNo;
                depremKayit.Soyadi = model.Soyadi;
                depremKayit.TcNo = model.TcNo;

                _context.SaveChanges();

                return new ResultJson<DepremKayit> { Success = true, Message = "Kayıt Başarılı.", Data = depremKayit };
            }
            catch (Exception ex)
            {
                return new ResultJson<DepremKayit> { Success = false, Message = "Hata : " + ex.Message };
            }
        }

        public async Task<List<VM_DepremKayit>> GetAll(Guid CompanyId)
        {
            var DepremKayit = _context.DepremKayits.Where(x => x.CompanyId == CompanyId).OrderBy(x=> x.Odano).ToList();
            var result = _mapper.Map<List<VM_DepremKayit>>(DepremKayit);
            return result;
        }
        public async Task<VM_DepremKayit> GetById(Guid Id)
        {
            var DepremKayit = _context.DepremKayits.Where(x => x.Id == Id).FirstOrDefault();
            var result = _mapper.Map<VM_DepremKayit>(DepremKayit);
            return result;
        }







    }
}
