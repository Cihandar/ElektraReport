using ElektraReport.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElektraReport.Interfaces.Cruds;
using ElektraReport.Applications.Auths.ViewModels;
using DNTCaptcha.Core;
using ElektraReport.Models.ResultModels;
using ElektraReport.Components;

namespace ElektraReport.Controllers
{
    [AllowAnonymous]
    [Route("Auth")]
    public class AuthController : Controller
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private IAuthCrud _authCrud;
        public AuthController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, IAuthCrud authCrud)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _authCrud = authCrud;
           
        }

        [Route("Login")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(VM_LoginModel model)
        {
            model.ClientIp = HttpContext?.Request?.HttpContext?.Connection?.RemoteIpAddress?.ToString();
            var result = await _authCrud.Login(model);

            if (result.Success)
            {
                UserSession userSession = new UserSession();
                await userSession.Set(result.Data, HttpContext);
            }

            return Json(result);
        }

        [Route("Register")]
        public IActionResult Register()
        {
            return View("Register");
        }

        [HttpPost("Register")]
        [ValidateAntiForgeryToken]
        //[ValidateDNTCaptcha(
        //    ErrorMessage = "Güvenlik Kodu Hatalı.. Lütfen Doğru Güvenlik Kodunu Giriniz..",
        //    CaptchaGeneratorLanguage = Language.Turkish,
        //    CaptchaGeneratorDisplayMode = DisplayMode.ShowDigits)
        //    ]
        public async Task<IActionResult> Register(VM_AuthRegister model)
        {
            if(ModelState.IsValid)
            {
                model.ClientIp = HttpContext?.Request?.HttpContext?.Connection?.RemoteIpAddress?.ToString();
                var result = await _authCrud.Register(model);

                return Json(result);
            }
            return Json(new ResultJson<AppUser> { Success = false, Message = "Hata",Data=null });
        }

        [Route("Logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return View("index");
        }
    }
}
