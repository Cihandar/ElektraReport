using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElektraReport.Applications.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ElektraReport.Models;
using Microsoft.AspNetCore.Identity;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using ElektraReport.Infrastructures.AutoMapper;
using System.Reflection;
using ElektraReport.Interfaces.Cruds;
using ElektraReport.Applications.Auths.Commands;
using ElektraReport.Applications.Companys.Commands;
using ElektraReport.Interfaces.ElektraApis;
using ElektraReport.Applications.ElektraApis.Command;
using DNTCaptcha.Core;

namespace ElektraReport
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {

            services.Configure<CookiePolicyOptions>(opt =>
            {
                opt.CheckConsentNeeded = context => true;
                opt.MinimumSameSitePolicy = SameSiteMode.None;
            });

            #region DbContext
            services.AddDbContext<DatabaseContext>(options => options.UseSqlServer(Configuration.GetConnectionString("ElektraReportDb")));
            #endregion

            #region Applications
            services.AddScoped<IAuthCrud, AuthCrud>();
            services.AddScoped<ICompanyCrud, CompanyCrud>();
            services.AddScoped<IApiRequest, ApiRequest>();
            #endregion

       

            services.AddAutoMapper(new Assembly[] { typeof(AutoMapperProfile).GetTypeInfo().Assembly });

            services.AddIdentity<AppUser, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 2;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireDigit = false;
                options.Password.RequiredUniqueChars = 0;
                options.Lockout = new LockoutOptions { AllowedForNewUsers = false };
                options.Tokens.PasswordResetTokenProvider = TokenOptions.DefaultEmailProvider;
            }).AddEntityFrameworkStores<DatabaseContext>().AddDefaultTokenProviders();

            services.Configure<SecurityStampValidatorOptions>(options =>
            {
                options.ValidationInterval = TimeSpan.Zero;
            });

            services.ConfigureApplicationCookie(options =>
            {
              //  options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromDays(365);
                options.LoginPath = "/Auth/Login";
                options.LogoutPath = "/Auth/Logout";
                options.AccessDeniedPath = "/Auth/AccessDenied";

                //options.Cookie = new CookieBuilder
                //{
                //    IsEssential = true, // required for auth to work without explicit user consent; adjust to suit your privacy policy,
                //    Name = ".Ex.Session",
                //    HttpOnly = false,
                //    SecurePolicy = CookieSecurePolicy.Always,
                //    Expiration = TimeSpan.FromDays(365)
                //};

            });

            //services.ConfigureApplicationCookie(options =>
            //{
            //    options.Cookie.HttpOnly = true;
            //    options.ExpireTimeSpan = TimeSpan.FromHours(1);
            //    options.LoginPath = "/Auth/Login";
            //    options.LogoutPath = "/Auth/Logout";
            //    options.AccessDeniedPath = "/Auth/AccessDenied";
            //    options.Cookie = new CookieBuilder
            //    {
            //        IsEssential = true // required for auth to work without explicit user consent; adjust to suit your privacy policy
            //    };
            //});

            //Session ý Optimize Et  on
            //services.AddSession(options =>
            //{
            //    options.IdleTimeout = TimeSpan.FromMinutes(20);
            //    options.Cookie.HttpOnly = false;
            //    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            //    options.Cookie.Name = ".Seriden.Session";
            //});
            //Session ý Optimize Et off

            //Antiforgery Token i Optimize Et  on
            //services.AddAntiforgery(options =>
            //{
            //    options.Cookie.HttpOnly = false;
            //    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            //    options.Cookie.Name = ".Seriden.Token";
            //});
            //Antiforgery Token i Optimize Et off

            //services.Configure<CookiePolicyOptions>(options =>
            //{
            //    This lambda determines whether user consent for non - essential cookies is needed for a given request.
            //     options.CheckConsentNeeded = context => true;
            //     options.MinimumSameSitePolicy = Microsoft.AspNetCore.Http.SameSiteMode.None;
            //    options.ConsentCookie.Name = ".Seriden.CookiePolicy"; // 

            //});



            services.AddControllersWithViews().AddFluentValidation(x => x.RegisterValidatorsFromAssemblyContaining<Startup>());

            #region Captcha
            services.AddDNTCaptcha(opt =>
            opt.UseCookieStorageProvider().ShowThousandsSeparators(false).WithEncryptionKey("AzK")
            );
            #endregion

            //    services.AddControllersWithViews().AddRazorRuntimeCompilation();

            services.AddMvc(option => option.EnableEndpointRouting=false).SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {

            app.UseDeveloperExceptionPage();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //}

            //app.Run(async (context) =>
            //{
            //    await context.Response.WriteAsync("Hello World!");
            //});

            //app.UseRouting();
            //app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();           
           // app.UseAuthorization();
            app.UseAuthentication();


            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            #region Routing
            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapControllerRoute(
            //        name: "usernotfound",
            //        pattern: "/usernotfound",
            //        defaults: new { controller = "Error", action = "UserNotFound" });

            //    endpoints.MapControllerRoute(
            //        name: "notfound",
            //        pattern: "/notfound",
            //        defaults: new { controller = "Error", action = "NotFound" });

            //    endpoints.MapControllerRoute(
            //        name: "unauthorized",
            //        pattern: "/unauthorized",
            //        defaults: new { controller = "Auth", action = "Unauthorized" });

            //    endpoints.MapControllerRoute(
            //        name: "authentication",
            //        pattern: "/authentication",
            //        defaults: new { controller = "Auth", action = "Login" });

            //    endpoints.MapControllerRoute(
            //        name: "default",
            //        pattern: "{controller=Home}/{action=Index}/{id?}");
            //});
            #endregion
        }
    }
}
