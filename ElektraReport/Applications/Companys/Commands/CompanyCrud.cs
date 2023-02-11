using AutoMapper;
using ElektraReport.Applications.Auths.ViewModels;
using ElektraReport.Applications.Context;
using ElektraReport.Applications.Companys.ViewModels;
using ElektraReport.Interfaces.Cruds;
using ElektraReport.Models;
using ElektraReport.Models.ResultModels;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace ElektraReport.Applications.Companys.Commands
{
    public class CompanyCrud : ICompanyCrud
    {
        public DatabaseContext _context;
        public IMapper _mapper;

        public CompanyCrud(DatabaseContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ResultJson<Company>> Add(VM_Company model)
        {
            try
            {
                var company = _mapper.Map<Company>(model);
                _context.Companys.Add(company);
                _context.SaveChanges();
                return new ResultJson<Company> { Success = true, Message = "Firma kayıt edildi.", Data = company };
            }
            catch (Exception ex)
            {
                return new ResultJson<Company> { Success = false, Message = "Hata : " + ex.Message };
            }

        }

        public async Task<ResultJson<Company>> Update(VM_Company model)
        {
            try
            {
                var company = _context.Companys.FirstOrDefault(x => x.Id == model.Id);
                company.ApiKey = model.ApiKey;
                company.ApiUrl = model.ApiUrl;
                company.CompanyName = model.CompanyName;
                company.Password = model.Password;
                company.Username = model.Username;

                _context.SaveChanges();

                return new ResultJson<Company> { Success = true, Message = "Kayıt Başarılı.", Data = company };
            }
            catch (Exception ex)
            {
                return new ResultJson<Company> { Success = false, Message = "Hata : " + ex.Message };
            }
        }

        public async Task<List<VM_Company>> GetAll(Guid CompanyId)
        {
            var company = _context.Companys.Where(x => x.IsDeleted == null || x.IsDeleted!=true).OrderBy(x=> x.CompanyName).ToList();
            var result = _mapper.Map<List<VM_Company>>(company);
            return result;
        }
        public async Task<VM_Company> GetById(Guid Id)
        {
            var company = _context.Companys.Where(x => x.Id == Id).FirstOrDefault();
            var result = _mapper.Map<VM_Company>(company);
            return result;
        }







    }
}
