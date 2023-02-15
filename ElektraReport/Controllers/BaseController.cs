using ElektraReport.Applications.Companys.Commands;
using ElektraReport.Components;
using ElektraReport.Interfaces.Cruds;
using ElektraReport.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ElektraReport.Controllers
{
    [Authorize]
    public class BaseController : Controller
    {
        public Guid CompanyId { get; set; }
        public string CompanyName { get; set; }
        public bool Admin { get; set; } = false;
        public bool OnayYetki { get; set; } = false;

        public override void OnActionExecuting(ActionExecutingContext context)
        {

            if (context.HttpContext.User.Identity.IsAuthenticated)
            {
                if (context.HttpContext.Session.GetString("User") != null)
                {
                    AppUser appUser = JsonConvert.DeserializeObject<AppUser>(context.HttpContext.Session.GetString("User").ToString());
                    if (appUser == null || appUser.CompanyId == Guid.Empty)
                    {
                        context.Result = new RedirectResult("/Auth/Logout");
                        return;
                    }

                    CompanyId = appUser.CompanyId;
                    Admin = appUser.Admin;
                    ViewBag.Admin = appUser.Admin;
                    ViewBag.CompanyId = appUser.CompanyId;
                    OnayYetki = appUser.OnayYetki;
                }
                else
                {
                    context.Result = new RedirectResult("/Auth/Logout");
                    return;
                }
            }

        }
    }
}

