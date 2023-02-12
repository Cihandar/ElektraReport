using ElektraReport.Applications.Companys.Commands;
using ElektraReport.Interfaces.Cruds;
using ElektraReport.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

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
         
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context,
                                         ActionExecutionDelegate next)
        {
     

            await next(); // the actual action

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
                    Admin = user.Admin;
                    ViewBag.Admin = Admin;
                    var companyCrud = context.HttpContext.RequestServices.GetService<ICompanyCrud>();
                    // logic before action goes here
                    var data = await companyCrud.GetById(CompanyId);
                    if (data != null)
                    ViewBag.CompanyName = data.CompanyName;

                    //ViewBag.PermissionEnum = user.Yetki;
                    //ViewBag.Admin = user.admin;
                    //admin = user.admin;
                    //_onlineUser.NameSurname = user.Name;
                    //_onlineUser.ProfilePicture = user.AvatarUrl;
                    //_onlineUser.RestaurantId = user.RestorantId;
                }

            }

  
            // logic after the action goes here
        }
    }
}
