using ElektraReport.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ElektraReport.Components
{
    public class UserSession
    {
        public AppUser Get(HttpContext context)
        {
            AppUser User;
            if (context.Session.GetString("User") != null)
            {
                User = JsonConvert.DeserializeObject<AppUser>(context.Session.GetString("User").ToString());
                return User;
            }
            return null;
        }

        public async Task<bool> Set(AppUser model, HttpContext context)
        {

            context.Session.SetString("User", JsonConvert.SerializeObject(model));

            try
            {
                CookieOptions cookie = new CookieOptions();
                cookie.Expires = DateTime.Now.AddYears(10);
                context.Response.Cookies.Append("Username", model.UserName, cookie);
                context.Response.Cookies.Append("CompanyId", model.CompanyId.ToString(), cookie);
                context.Response.Cookies.Append("Admin", model.Admin.ToString(), cookie);

                //var authProperties = new AuthenticationProperties
                //{
                //    //AllowRefresh = <bool>,
                //    // Refreshing the authentication session should be allowed.

                //    //ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
                //    // The time at which the authentication ticket expires. A 
                //    // value set here overrides the ExpireTimeSpan option of 
                //    // CookieAuthenticationOptions set with AddCookie.

                //    //IsPersistent = true,
                //    // Whether the authentication session is persisted across 
                //    // multiple requests. When used with cookies, controls
                //    // whether the cookie's lifetime is absolute (matching the
                //    // lifetime of the authentication ticket) or session-based.

                //    //IssuedUtc = <DateTimeOffset>,
                //    // The time at which the authentication ticket was issued.

                //    //RedirectUri = <string>
                //    // The full path or absolute URI to be used as an http 
                //    // redirect response value.
                //};

                //await context.SignInAsync(CookieAuthenticationDefaults.CookiePrefix,new ClaimsPrincipal(claimsIdentity), authProperties);
                return true;
            }
            catch (Exception ex)
            {

                return false;
            }

        }
        public bool Delete(HttpContext context)
        {
            context.Session.Remove("User");
            return true;
        }
    }
}
