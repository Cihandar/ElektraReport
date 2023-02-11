using ElektraReport.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace ElektraReport.Controllers
{
    [Authorize]


    public class BaseController : Controller
    {
        public static Guid CompanyId { get; set; }
        public static string CompanyName { get; set; }
        public static bool Admin { get; set; }


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
                        Admin = user.Admin;
                        ViewBag.Admin = Admin;
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
