using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ElektraReport.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using ElektraReport.Applications.Companys.Commands;
using ElektraReport.Applications.Companys.ViewModels;

namespace ElektraReport.Controllers
{
    [Authorize]


    public class BaseController : Controller
    {
        public static Guid CompanyId { get; set; }
    

 
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.HttpContext.User.Identity.IsAuthenticated)
            {
                var _userManager = context.HttpContext.RequestServices.GetService<UserManager<AppUser>>();
                var user = _userManager.FindByEmailAsync(context.HttpContext.User.Identity.Name).Result;

                if (user == null)
                {
                    user = _userManager.FindByNameAsync(context.HttpContext.User.Identity.Name).Result;

                    if (user != null)
                    {
                        CompanyId = user.CompanyId;                    
                        return;
                    }

                    context.Result = new RedirectToRouteResult("Home");
                    return;
                }
                else
                {
                    CompanyId = user.CompanyId;
                    //ViewBag.PermissionEnum = user.Yetki;
                    //ViewBag.Admin = user.admin;
                    //admin = user.admin;
                    //_onlineUser.NameSurname = user.Name;
                    //_onlineUser.ProfilePicture = user.AvatarUrl;
                    //_onlineUser.RestaurantId = user.RestorantId;
                }

            }
        }
    }
}
