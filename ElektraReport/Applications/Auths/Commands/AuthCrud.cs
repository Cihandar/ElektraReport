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
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace ElektraReport.Applications.Auths.Commands
{
    public class AuthCrud : IAuthCrud
    {
        public DatabaseContext _context;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private ICompanyCrud _company;
        private ISendEmail _mail;
        private IFluentMailCore _fluentmail;
        public IMapper _mapper;

        public AuthCrud(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, DatabaseContext context, ICompanyCrud company, ISendEmail mail, IFluentMailCore fluentmail, IMapper mapper)
        {
            _context = context;
            _signInManager = signInManager;
            _userManager = userManager;
            _company = company;
            _mail = mail;
            _fluentmail = fluentmail;
            _mapper = mapper;
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
                    FullName = model.Password,
                    ClientIp = model.ClientIp
                };
                var resultuser = await _userManager.CreateAsync(user, model.Password);
                if (resultuser.Succeeded)
                {
                    var resultmail = await _mail.Send(model.Email, "", "", model.Email, model.Password, "");
                    // var resultmail = await _fluentmail.Send(model.Email, model.Password);
                    //var resultemail = await _email.Send(model.Email, "Alze E Portal Giriş Bilgileri", "", model.Email, sifre.ToString(), "IlkKayit");
                    if (resultmail.Success)
                    {
                        _context.Logs.Add(
                            new Log
                            {
                                ClientIp = model.ClientIp,
                                Note = "Kayıt Oldu",
                                Tarih = DateTime.Now,
                                UserId = user.Id,
                                UserName = user.Email,
                                CompanyId = user.CompanyId
                            }
                            );
                        _context.SaveChanges();
                        return new ResultJson<AppUser> { Success = true, Message = "<h3 class='text-danger'>Şifreniz : " + model.Password + "</h3> <br>Kaydınız yapıldı. Size Vermiş olduğumuz şifreyi not almayı unutmayın. bu şifre ile giriş yapabilirsiniz.", Data = user };
                    }
                    else
                    {
                        return new ResultJson<AppUser> { Success = false, Message = "Şifresi : " + model.Password + " Kaydınız yapıldı.  Size Vermiş olduğumuz şifreyi not almayı unutmayın. bu şifre ile giriş yapabilirsiniz.", Data = user };
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

            user.ClientIp = model.ClientIp;
            await _context.SaveChangesAsync();

            if (user.Status == 0)
                return new ResultJson<AppUser> { Success = false, Message = "Kullanıcınız onaylanmamış, Yetkili kişiler sizi aricak!", Data = user };
            if (user.Status > 1)
                return new ResultJson<AppUser> { Success = false, Message = "Kullanıcınız onaylanmamıştır !", Data = user };

            var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RemenberMe, false);
            if (result.Succeeded)
            {
                _context.Logs.Add(
            new Log
            {
                ClientIp = model.ClientIp,
                Note = "Giriş Yaptı",
                Tarih = DateTime.Now,
                UserId = user.Id,
                UserName = user.Email,
                CompanyId = user.CompanyId
            }
            );
                _context.SaveChanges();
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

        public async Task<ResultJson<AppUser>> SetActiveUser(string email)
        {
            try
            {
                var user = await _userManager.Users.Where(x => x.UserName == email).FirstOrDefaultAsync();
                if (user != null)
                {
                    user.Status = 1;
                    await _context.SaveChangesAsync();
                }

                return new ResultJson<AppUser> { Success = true, Message = "Kayıt Başarılı." };
            }
            catch (Exception ex)
            {
                return new ResultJson<AppUser> { Success = false, Message = "Hata : " + ex.Message };
            }
        }

        public async Task<ResultJson<AppUser>> SetDisableUser(string email)
        {
            try
            {
                var user = await _userManager.Users.Where(x => x.UserName == email).FirstOrDefaultAsync();
                if (user != null)
                {
                    user.Status = 2;
                    await _context.SaveChangesAsync();
                }

                return new ResultJson<AppUser> { Success = true, Message = "İşlem Başarılı." };
            }
            catch (Exception ex)
            {
                return new ResultJson<AppUser> { Success = false, Message = "Hata : " + ex.Message };
            }
        }

        public async Task<List<VM_PassiveUsers>> GetAllPassiveUsers()
        {
            List<VM_PassiveUsers> response = new List<VM_PassiveUsers>();
            var users = await _context.Users.Where(x => x.Status == 0).ToListAsync();
            foreach (var item in users)
            {
                var company = _context.Companys.Where(x => x.Id == item.CompanyId).FirstOrDefault();
                if (company != null)
                {
                    response.Add(new VM_PassiveUsers
                    {
                        Company = company.CompanyName,
                        Email = item.Email,
                        Name = company.Name,
                        FullName = item.FullName,
                        Phone = company.Phone
                    });
                }
            }
            return response;
        }

    }
}
