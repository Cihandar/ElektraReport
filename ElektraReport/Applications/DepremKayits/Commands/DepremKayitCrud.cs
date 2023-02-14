using AutoMapper;
using ElektraReport.Applications.Context;
using ElektraReport.Applications.DepremKayits.ViewModels;
using ElektraReport.Interfaces.Cruds;
using ElektraReport.Models;
using ElektraReport.Models.ResultModels;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
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
        private IConfiguration config;

        public DepremKayitCrud(DatabaseContext context, IMapper mapper, IConfiguration config)
        {
            _context = context;
            _mapper = mapper;
            this.config = config;
        }

        public async Task<ResultJson<DepremKayit>> Add(VM_DepremKayit model)
        {
            try
            {
                model.isCheckOut = false;
                model.CreateDate = DateTime.UtcNow;
                model.BlackList = false;
                model.BlackListNote = "";
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
                    var snc = await _context.DepremKayits.Where(x => x.Odano == item.Odano && x.TcNo == item.TcNo && x.CompanyId == item.CompanyId && (x.IsDeleted == null || x.IsDeleted != true)).FirstOrDefaultAsync();
                    var res = snc == null ? await Add(item) : null;
                    response.Add(res);
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
                depremKayit.BlackList = model.BlackList;
                depremKayit.BlackListNote = model.BlackListNote;
                depremKayit.isCheckOut = model.isCheckOut;

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
            var DepremKayit = _context.DepremKayits.Where(x => x.CompanyId == CompanyId && (x.IsDeleted == null || x.IsDeleted != true)).OrderBy(x => x.Odano).ToList();
            var result = _mapper.Map<List<VM_DepremKayit>>(DepremKayit);
            return result;
        }

        public async Task<VM_DepremKayit> GetById(Guid Id)
        {
            var DepremKayit = _context.DepremKayits.Where(x => x.Id == Id && (x.IsDeleted == null || x.IsDeleted != true)).FirstOrDefault();
            var result = _mapper.Map<VM_DepremKayit>(DepremKayit);
            return result;
        }

        public async Task<bool> Delete(Guid Id,string ip)
        {
            try
            {
                var depremKayit = _context.DepremKayits.FirstOrDefault(x => x.Id == Id);
                depremKayit.ModifyClientIp = ip;
                depremKayit.DeleteTime = DateTime.UtcNow;
                depremKayit.IsDeleted = true;
                _context.SaveChanges();
                return true;
            }
            catch (Exception)
            {

                return false;
            }

        }


        public async Task<VM_Dashboard> Dashboards()
        {
            try
            {
               var depremList = new  VM_Dashboard();
                depremList.DepremDashboard = new List<VM_DepremDashBoard>();
                SqlConnection con = new SqlConnection(config.GetConnectionString("ElektraReportDb"));
                SqlCommand cmd = new SqlCommand("select * from Q_DashBoard", con);
                con.Open();
                var dr = cmd.ExecuteReader();
                while(dr.Read())
                {
                    var deprem = new VM_DepremDashBoard();
                    deprem.CompanyId =   dr.GetFieldValue<Guid>(dr.GetOrdinal("Id"));
                    deprem.Name = dr.GetFieldValue<string>(dr.GetOrdinal("CompanyName"));
                    deprem.MevcutKisi = dr.GetFieldValue<int>(dr.GetOrdinal("mevcutkisi"));
                    deprem.MevcutOda = dr.GetFieldValue<int>(dr.GetOrdinal("mevcutoda"));
                    deprem.CikanOda = dr.GetFieldValue<int>(dr.GetOrdinal("cikanoda"));
                    deprem.CikanKisi = dr.GetFieldValue<int>(dr.GetOrdinal("cikankisi"));
                    depremList.DepremDashboard.Add(deprem);
                }
                con.Close();
                 var company = _context.Companys.ToList();
                depremList.TotalCompany = company.Count;
                depremList.DepremDashboard = depremList.DepremDashboard.OrderByDescending(x => x.MevcutKisi).ToList();
                //var deprem = _context.DepremKayits.Where(x => (x.IsDeleted == null || x.IsDeleted != true)).AsNoTracking().ToList();
                //var depremKayit = deprem.GroupBy(x => new { x.OtelAdi,x.CompanyId,x.isCheckOut }).Select(y =>

                //new VM_DepremKayitDashboard
                //{
                //    Name = y.Key.OtelAdi,
                //    CompanyId = y.Key.CompanyId,
                //    isCheckOut = y.Key.isCheckOut,
                //    UserTotal = y.Count()
                //}).ToList();

                //foreach (var item in depremKayit)
                //{
                //    item.CompanyTotal = company.Count;
                //    item.RoomTotal = deprem.Where(x => x.OtelAdi == item.Name && x.isCheckOut!=true && (x.IsDeleted == null || x.IsDeleted != true)).GroupBy(x =>  x.Odano).Select(x => x.Key).Count();
                //    item.CheckOutRoomTotal = deprem.Where(x => x.OtelAdi == item.Name && x.isCheckOut==true && (x.IsDeleted == null || x.IsDeleted != true)).GroupBy(x => x.Odano).Select(x => x.Key).Count();
                //}

                //var result = _mapper.Map<List<VM_DepremKayitDashboard>>(depremKayit);
                return depremList;
            }
            catch (Exception ex)
            {

                throw;
            }


        }


        public async Task<List<VM_DepremKayit>> GetAllOtel(Guid companyId, string adsoyad, string tcno)
        {
            var DepremKayit = new List<DepremKayit>();

            var ad = "";
            var soyad = "";
            if (!string.IsNullOrEmpty(adsoyad))
            {
                var splt = adsoyad.Split(' ');
                if (splt.Length == 1)
                {
                    ad = adsoyad;
                    soyad = adsoyad;
                }
                if (splt.Length == 2)
                {
                    ad = splt[0];
                    soyad = splt[1];
                }
                if (splt.Length == 3)
                {
                    ad = splt[0] + " " + splt[1];
                    soyad = splt[2];
                }
                if (splt.Length > 3)
                {
                    ad = adsoyad;
                    soyad = adsoyad;
                }
            }
            if (Guid.Empty == companyId)
            {
                if (!string.IsNullOrEmpty(adsoyad) && !string.IsNullOrEmpty(tcno))
                    DepremKayit = _context.DepremKayits.Where(x => (x.IsDeleted == null || x.IsDeleted != true) && (x.Adi.Contains(ad) || x.Soyadi.Contains(soyad)) && x.TcNo.Contains(tcno)).OrderBy(x => x.Odano).ToList();
                else if (!string.IsNullOrEmpty(adsoyad))
                    DepremKayit = _context.DepremKayits.Where(x => (x.IsDeleted == null || x.IsDeleted != true) && (x.Adi.Contains(ad) || x.Soyadi.Contains(soyad))).OrderBy(x => x.Odano).ToList();
                else if (!string.IsNullOrEmpty(tcno))
                    DepremKayit = _context.DepremKayits.Where(x => (x.IsDeleted == null || x.IsDeleted != true) && x.TcNo.Contains(tcno)).OrderBy(x => x.Odano).ToList();
                else
                    DepremKayit = _context.DepremKayits.Where(x => (x.IsDeleted == null || x.IsDeleted != true)).OrderBy(x => x.Odano).ToList();
            }
            else
            {
                if (!string.IsNullOrEmpty(adsoyad) && !string.IsNullOrEmpty(tcno))
                    DepremKayit = _context.DepremKayits.Where(x => x.CompanyId == companyId && (x.IsDeleted == null || x.IsDeleted != true) && (x.Adi.Contains(ad) || x.Soyadi.Contains(soyad)) && x.TcNo.Contains(tcno)).OrderBy(x => x.Odano).ToList();
                else if (!string.IsNullOrEmpty(adsoyad))
                    DepremKayit = _context.DepremKayits.Where(x => x.CompanyId == companyId && (x.IsDeleted == null || x.IsDeleted != true) && (x.Adi.Contains(ad) || x.Soyadi.Contains(soyad))).OrderBy(x => x.Odano).ToList();
                else if (!string.IsNullOrEmpty(tcno))
                    DepremKayit = _context.DepremKayits.Where(x => x.CompanyId == companyId && (x.IsDeleted == null || x.IsDeleted != true) && x.TcNo.Contains(tcno)).OrderBy(x => x.Odano).ToList();
                else
                    DepremKayit = _context.DepremKayits.Where(x => x.CompanyId == companyId && (x.IsDeleted == null || x.IsDeleted != true)).OrderBy(x => x.Odano).ToList();
            }

            var result = _mapper.Map<List<VM_DepremKayit>>(DepremKayit);

            return result;

        }

        public async Task<List<VM_DepremKayit>> GetAllOtelBlackList(Guid companyId, string adsoyad, string tcno)
        {
            var DepremKayit = new List<DepremKayit>();

            var ad = "";
            var soyad = "";
            if (!string.IsNullOrEmpty(adsoyad))
            {
                var splt = adsoyad.Split(' ');
                if (splt.Length == 1)
                {
                    ad = adsoyad;
                    soyad = adsoyad;
                }
                if (splt.Length == 2)
                {
                    ad = splt[0];
                    soyad = splt[1];
                }
                if (splt.Length == 3)
                {
                    ad = splt[0] + " " + splt[1];
                    soyad = splt[2];
                }
                if (splt.Length > 3)
                {
                    ad = adsoyad;
                    soyad = adsoyad;
                }
            }
            if (Guid.Empty == companyId)
            {
                if (!string.IsNullOrEmpty(adsoyad) && !string.IsNullOrEmpty(tcno))
                    DepremKayit = _context.DepremKayits.Where(x => x.BlackList==true && (x.IsDeleted == null || x.IsDeleted != true) && (x.Adi.Contains(ad) || x.Soyadi.Contains(soyad)) && x.TcNo.Contains(tcno)).OrderBy(x => x.Odano).ToList();
                else if (!string.IsNullOrEmpty(adsoyad))
                    DepremKayit = _context.DepremKayits.Where(x => x.BlackList == true && (x.IsDeleted == null || x.IsDeleted != true) && (x.Adi.Contains(ad) || x.Soyadi.Contains(soyad))).OrderBy(x => x.Odano).ToList();
                else if (!string.IsNullOrEmpty(tcno))
                    DepremKayit = _context.DepremKayits.Where(x => x.BlackList == true && (x.IsDeleted == null || x.IsDeleted != true) && x.TcNo.Contains(tcno)).OrderBy(x => x.Odano).ToList();
                else
                    DepremKayit = _context.DepremKayits.Where(x => x.BlackList == true && (x.IsDeleted == null || x.IsDeleted != true)).OrderBy(x => x.Odano).ToList();
            }
            else
            {
                if (!string.IsNullOrEmpty(adsoyad) && !string.IsNullOrEmpty(tcno))
                    DepremKayit = _context.DepremKayits.Where(x => x.BlackList == true && x.CompanyId == companyId && (x.IsDeleted == null || x.IsDeleted != true) && (x.Adi.Contains(ad) || x.Soyadi.Contains(soyad)) && x.TcNo.Contains(tcno)).OrderBy(x => x.Odano).ToList();
                else if (!string.IsNullOrEmpty(adsoyad))
                    DepremKayit = _context.DepremKayits.Where(x => x.BlackList == true && x.CompanyId == companyId && (x.IsDeleted == null || x.IsDeleted != true) && (x.Adi.Contains(ad) || x.Soyadi.Contains(soyad))).OrderBy(x => x.Odano).ToList();
                else if (!string.IsNullOrEmpty(tcno))
                    DepremKayit = _context.DepremKayits.Where(x => x.BlackList == true && x.CompanyId == companyId && (x.IsDeleted == null || x.IsDeleted != true) && x.TcNo.Contains(tcno)).OrderBy(x => x.Odano).ToList();
                else
                    DepremKayit = _context.DepremKayits.Where(x => x.BlackList == true && x.CompanyId == companyId && (x.IsDeleted == null || x.IsDeleted != true)).OrderBy(x => x.Odano).ToList();
            }

            var result = _mapper.Map<List<VM_DepremKayit>>(DepremKayit);

            return result;

        }

        public async Task<bool> CheckOut(Guid Id,string ip)
        {
            try
            {
                var depremKayit = _context.DepremKayits.FirstOrDefault(x => x.Id == Id);
                depremKayit.ModifyClientIp = ip;
                depremKayit.ModifyDate = DateTime.UtcNow;
                depremKayit.isCheckOut = true;
                depremKayit.CikisTarihi = DateTime.UtcNow;
                _context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<List<VM_DepremKayitDashboard>> Dashboards2(Guid CompanyId)
        {
            try
            {
                var company = _context.Companys.ToList();

                var depremKayit = _context.DepremKayits.Where(x => (x.IsDeleted == null || x.IsDeleted != true) && x.CompanyId==CompanyId && x.isCheckOut!=true).GroupBy(x => new { x.OtelAdi, x.CompanyId }).Select(y =>

                 new VM_DepremKayitDashboard
                 {
                     Name = y.Key.OtelAdi,
                     CompanyId = y.Key.CompanyId,
                     UserTotal = y.Count()
                 }).ToList();

                foreach (var item in depremKayit)
                {
                    item.CompanyTotal = company.Count;
                    item.RoomTotal = _context.DepremKayits.Where(x => x.CompanyId == item.CompanyId && x.isCheckOut != true && (x.IsDeleted == null || x.IsDeleted != true)).GroupBy(x => x.Odano).Select(x => x.Key).Count();
                }

                var result = _mapper.Map<List<VM_DepremKayitDashboard>>(depremKayit);
                return result.OrderByDescending(x => x.UserTotal).ToList();
            }
            catch (Exception ex)
            {

                throw;
            }
        }

    
    }
}
