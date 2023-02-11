using ElektraReport.Models.ResultModels;
using FluentEmail.Core;
using FluentEmail.Smtp;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace ElektraReport.Infrastructures.Mail
{
    public class FluentMailCore : IFluentMailCore
    {
        private IFluentEmail _fluentEmail;

        public FluentMailCore(IFluentEmail fluentEmail)
        {
            _fluentEmail = fluentEmail;
        }

        public async Task<ResultJson2> Send(string email,string code)
        {
            try
            {
                var  fluentEmail = await Email
                .From("depremyardim@alzeteknoloji.com")
                .To(email)
                .Subject("Rezervasyon Kayit İçin Giriş Bilgileri")
                .Tag("Rezervasyon Kayit İçin Giriş Bilgileri")
                .Body("Kullanıcı Adınız : " + email +"<br> Şifre : "+code+"<br> Link:http://deprem.alzeportal.com")
                .SendAsync();
                return new ResultJson2 { Success =true };
                //var sender = new SmtpSender(() => new SmtpClient("smtp.gmail.com")
                //{
                //    UseDefaultCredentials = false,
                //    Port = 587,
                //    Credentials = new NetworkCredential("duhangokhan@gmail.com", "190702dkn"),
                //    EnableSsl = false,
                //});

                //Email.DefaultSender = sender;
                //var email2 = Email
                //    .From("duhangokhan@gmail.com", "Mark's Coding Spot")
                //    .To("hidddencode@gmail.com", "Mark Melton")
                //    .Subject("Hey there!")
                //    .Body("Just to holla at you real quick.");


                //var response = await email2.SendAsync();
            }
            catch (Exception ex)
            {
                return new ResultJson2 { Success = false, Message = ex.Message };
            }         
        }
    }

}
