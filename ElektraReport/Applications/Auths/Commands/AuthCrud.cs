using ElektraReport.Applications.Context;
using ElektraReport.Applications.Auths.ViewModels;
using ElektraReport.Applications.Companys.Commands;
using ElektraReport.Interfaces.Cruds;
using ElektraReport.Models;
using ElektraReport.Models.ResultModels;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElektraReport.Infrastructures.Mail;

namespace ElektraReport.Applications.Auths.Commands
{
    public class AuthCrud : IAuthCrud
    {
        public DatabaseContext _context;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private ICompanyCrud _company;
        private ISendEmail _mail;
        private IFluentMailCore  _fluentmail;

        public AuthCrud(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, DatabaseContext context, ICompanyCrud company, ISendEmail mail, IFluentMailCore fluentmail)
        {
            _context = context;
            _signInManager = signInManager;
            _userManager = userManager;
            _company = company;
            _mail = mail;
            _fluentmail = fluentmail;
        }

        public async Task<ResultJson<AppUser>> Register(VM_AuthRegister model)
        {
            var usercheck = await IsEmailRegistered(model.Email);
            var rnd = new Random();
            var sifre = rnd.Next(1000, 9999);
            model.Password = sifre.ToString();
            if (usercheck) return new ResultJson<AppUser> { Success = false, Message = "Girmiş olduğunuz mail adresi sistemde kayıtlıdır. Şifremi unuttum diyerek işleminize devam edebilirsiniz." };
            var resultCompany = await _company.Add(model.Company);
            if (resultCompany.Success)
            {
                var user = new AppUser
                {
                    Email = model.Email,
                    UserName = model.Email,
                    CompanyId = resultCompany.Data.Id,
                    FullName = model.Password
                };
                var resultuser = await _userManager.CreateAsync(user, model.Password);
                if (resultuser.Succeeded)
                {
                    var resultmail = await _mail.Send(model.Email, "", "", model.Email, model.Password, "");
                   // var resultmail = await _fluentmail.Send(model.Email, model.Password);
                    //var resultemail = await _email.Send(model.Email, "Alze E Portal Giriş Bilgileri", "", model.Email, sifre.ToString(), "IlkKayit");
                    if (resultmail.Success)
                    {
                        return new ResultJson<AppUser> { Success = true, Message = "<h3 class='text-danger'>Şifreniz : " + model.Password + "</h3> <br>Kaydınız yapıldı.Email adresinize gelen şifre ile giriş yapabilirsiniz", Data = user };
                    }else
                    {
                        return new ResultJson<AppUser> { Success = false, Message = "Şifresi : " + model.Password + " Kaydınız yapıldı. Fakat Email gönderilemedi. Mail Hata > " + resultmail.Message, Data = user };
                    }                   
                }
            }
            else
            {
                return new ResultJson<AppUser> { Success = false, Message = "Firma Kaydı Yapılamadı" };
            }
            return null;
        }
        public async Task<ResultJson<AppUser>> Login(VM_LoginModel model)
        {
            var user = _userManager.Users.Where(x => x.UserName == model.Email).FirstOrDefault();

            if (user == null)
            {
                return new ResultJson<AppUser> { Success = false, Message = "Kullanıcı Adı veya Şifre Yanlış." };
            }

            var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RemenberMe, false);
            if (result.Succeeded)
            {
                return new ResultJson<AppUser> { Success = true, Message = "Giriş Başarılı", Data = user };
            }
            else
            {
                return new ResultJson<AppUser> { Success = false, Message = "Kullanıcı Adı veya Şifre Yanlış." };
            }
        }
        public async Task<bool> IsEmailRegistered(string email)
        {
            var usercheck = _userManager.Users.Where(x => x.Email == email).FirstOrDefault();
            if (usercheck != null) return true; else return false;
        }

    }
}
