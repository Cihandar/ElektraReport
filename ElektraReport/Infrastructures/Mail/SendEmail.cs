﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;
using System.Text;
using Microsoft.Extensions.Configuration;
using ElektraReport.Models;
using ElektraReport.Models.ResultModels;

namespace ElektraReport.Infrastructures.Mail
{
    public class SendEmail : ISendEmail
    {

        IConfiguration _config;

        public SendEmail(IConfiguration config)
        {
            _config = config;
        }

        public async Task<ResultJson2> Send(string to, string subject, string message, string name, string Password, string template)
        {


            var emailSettings = _config.GetSection("Email").Get<EmailSettings>();

           // var content = Encoding.UTF8.GetString(Convert.FromBase64String(emailtemplate(template)));


            //content = content.Replace("[UserName]", name);
            //content = content.Replace("[Password]", Password);
            //content = content.Replace("[Link]", "https://www.alzeteknoloji.com");

            var content = "Kullanıcı Adınız : " + name + "<br> Şifre : " + Password + "<br> Link:http://deprem.alzeportal.com";
            //if (template != "newcontact") content = content.Replace("[sifre]", message); else content = content.Replace("[mesaj]", message);
            //content = content.Replace("[tarih]", DateTime.Now.ToString("dd.MM.yyyy"));

            var LoginInfo = new NetworkCredential(emailSettings.UserName, emailSettings.Password);
            var client = new SmtpClient { Port = emailSettings.Port, Host = emailSettings.Smtp, EnableSsl = emailSettings.Ssl, Credentials = LoginInfo };

            var mail = new MailMessage
            {
                From = new MailAddress("depremyardim@alzeportal.com", subject),
                Subject = "Deprem Rezervasyon Kayıt Portalı - Giriş Bilgileri",
                Body = content,
                IsBodyHtml = true
            };

            mail.To.Add(to);  // Gidecek adres ekleniyor

            try
            {
                client.Send(mail);
                return new ResultJson2 { Success = true };
            }
            catch (Exception ex)
            {

                return new ResultJson2 { Success = false, Message = ex.Message };
            }

        }

        string emailtemplate(string typemail)
        {
            switch (typemail)
            {
                case "IlkKayit":

                    return "PCFET0NUWVBFIGh0bWw+CjxodG1sPgoKPGhlYWQ+CiAgICA8dGl0bGU+QWx6ZSBFIFBvcnRhbDwvdGl0bGU+CiAgICA8bWV0YSBodHRwLWVxdWl2PSJDb250ZW50LVR5cGUiIGNvbnRlbnQ9InRleHQvaHRtbDsgY2hhcnNldD11dGYtOCIgLz4KICAgIDxtZXRhIG5hbWU9InZpZXdwb3J0IiBjb250ZW50PSJ3aWR0aD1kZXZpY2Utd2lkdGgsIGluaXRpYWwtc2NhbGU9MSI+CiAgICA8bWV0YSBodHRwLWVxdWl2PSJYLVVBLUNvbXBhdGlibGUiIGNvbnRlbnQ9IklFPWVkZ2UiIC8+CiAgICA8c3R5bGUgdHlwZT0idGV4dC9jc3MiPgogICAgICAgIGJvZHksCiAgICAgICAgdGFibGUsCiAgICAgICAgdGQsCiAgICAgICAgYSB7CiAgICAgICAgICAgIC13ZWJraXQtdGV4dC1zaXplLWFkanVzdDogMTAwJTsKICAgICAgICAgICAgLW1zLXRleHQtc2l6ZS1hZGp1c3Q6IDEwMCU7CiAgICAgICAgfQoKICAgICAgICB0YWJsZSwKICAgICAgICB0ZCB7CiAgICAgICAgICAgIG1zby10YWJsZS1sc3BhY2U6IDBwdDsKICAgICAgICAgICAgbXNvLXRhYmxlLXJzcGFjZTogMHB0OwogICAgICAgIH0KCiAgICAgICAgaW1nIHsKICAgICAgICAgICAgLW1zLWludGVycG9sYXRpb24tbW9kZTogYmljdWJpYzsKICAgICAgICB9CgogICAgICAgIGltZyB7CiAgICAgICAgICAgIGJvcmRlcjogMDsKICAgICAgICAgICAgaGVpZ2h0OiBhdXRvOwogICAgICAgICAgICBsaW5lLWhlaWdodDogMTAwJTsKICAgICAgICAgICAgb3V0bGluZTogbm9uZTsKICAgICAgICAgICAgdGV4dC1kZWNvcmF0aW9uOiBub25lOwogICAgICAgIH0KCiAgICAgICAgdGFibGUgewogICAgICAgICAgICBib3JkZXItY29sbGFwc2U6IGNvbGxhcHNlICFpbXBvcnRhbnQ7CiAgICAgICAgfQoKICAgICAgICBib2R5IHsKICAgICAgICAgICAgaGVpZ2h0OiAxMDAlICFpbXBvcnRhbnQ7CiAgICAgICAgICAgIG1hcmdpbjogMCAhaW1wb3J0YW50OwogICAgICAgICAgICBwYWRkaW5nOiAwICFpbXBvcnRhbnQ7CiAgICAgICAgICAgIHdpZHRoOiAxMDAlICFpbXBvcnRhbnQ7CiAgICAgICAgfQoKICAgICAgICBhW3gtYXBwbGUtZGF0YS1kZXRlY3RvcnNdIHsKICAgICAgICAgICAgY29sb3I6IGluaGVyaXQgIWltcG9ydGFudDsKICAgICAgICAgICAgdGV4dC1kZWNvcmF0aW9uOiBub25lICFpbXBvcnRhbnQ7CiAgICAgICAgICAgIGZvbnQtc2l6ZTogaW5oZXJpdCAhaW1wb3J0YW50OwogICAgICAgICAgICBmb250LWZhbWlseTogaW5oZXJpdCAhaW1wb3J0YW50OwogICAgICAgICAgICBmb250LXdlaWdodDogaW5oZXJpdCAhaW1wb3J0YW50OwogICAgICAgICAgICBsaW5lLWhlaWdodDogaW5oZXJpdCAhaW1wb3J0YW50OwogICAgICAgIH0KCiAgICAgICAgQG1lZGlhIHNjcmVlbiBhbmQgKG1heC13aWR0aDogNDgwcHgpIHsKICAgICAgICAgICAgLm1vYmlsZS1oaWRlIHsKICAgICAgICAgICAgICAgIGRpc3BsYXk6IG5vbmUgIWltcG9ydGFudDsKICAgICAgICAgICAgfQoKICAgICAgICAgICAgLm1vYmlsZS1jZW50ZXIgewogICAgICAgICAgICAgICAgdGV4dC1hbGlnbjogY2VudGVyICFpbXBvcnRhbnQ7CiAgICAgICAgICAgIH0KICAgICAgICB9CgogICAgICAgIGRpdltzdHlsZSo9Im1hcmdpbjogMTZweCAwOyJdIHsKICAgICAgICAgICAgbWFyZ2luOiAwICFpbXBvcnRhbnQ7CiAgICAgICAgfQogICAgPC9zdHlsZT4KCjxib2R5IHN0eWxlPSJtYXJnaW46IDAgIWltcG9ydGFudDsgcGFkZGluZzogMCAhaW1wb3J0YW50OyBiYWNrZ3JvdW5kLWNvbG9yOiAjZWVlZWVlOyIgYmdjb2xvcj0iI2VlZWVlZSI+CiAKICAgIDx0YWJsZSBib3JkZXI9IjAiIGNlbGxwYWRkaW5nPSIwIiBjZWxsc3BhY2luZz0iMCIgd2lkdGg9IjEwMCUiPgogICAgICAgIDx0cj4KICAgICAgICAgICAgPHRkIGFsaWduPSJjZW50ZXIiIHN0eWxlPSJiYWNrZ3JvdW5kLWNvbG9yOiAjZWVlZWVlOyIgYmdjb2xvcj0iI2VlZWVlZSI+CiAgICAgICAgICAgICAgICA8dGFibGUgYWxpZ249ImNlbnRlciIgYm9yZGVyPSIwIiBjZWxscGFkZGluZz0iMCIgY2VsbHNwYWNpbmc9IjAiIHdpZHRoPSIxMDAlIiBzdHlsZT0ibWF4LXdpZHRoOjYwMHB4OyI+CiAgICAgICAgICAgICAgICAgICAgPHRyPgogICAgICAgICAgICAgICAgICAgICAgICA8dGQgYWxpZ249ImNlbnRlciIgdmFsaWduPSJ0b3AiIHN0eWxlPSJmb250LXNpemU6MDsgcGFkZGluZzogMzVweDsiIGJnY29sb3I9IiNGNDQzMzYiPgogICAgICAgICAgICAgICAgICAgICAgICAgICAgPGRpdiBzdHlsZT0iZGlzcGxheTppbmxpbmUtYmxvY2s7IG1heC13aWR0aDo1MCU7IG1pbi13aWR0aDoxMDBweDsgdmVydGljYWwtYWxpZ246dG9wOyB3aWR0aDoxMDAlOyI+CiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgPHRhYmxlIGFsaWduPSJsZWZ0IiBib3JkZXI9IjAiIGNlbGxwYWRkaW5nPSIwIiBjZWxsc3BhY2luZz0iMCIgd2lkdGg9IjEwMCUiIHN0eWxlPSJtYXgtd2lkdGg6MzAwcHg7Ij4KICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgPHRyPgogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgPHRkIGFsaWduPSJsZWZ0IiB2YWxpZ249InRvcCIgc3R5bGU9ImZvbnQtZmFtaWx5OiBPcGVuIFNhbnMsIEhlbHZldGljYSwgQXJpYWwsIHNhbnMtc2VyaWY7IGZvbnQtc2l6ZTogMzZweDsgZm9udC13ZWlnaHQ6IDgwMDsgbGluZS1oZWlnaHQ6IDQ4cHg7IiBjbGFzcz0ibW9iaWxlLWNlbnRlciI+CiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgPGgxIHN0eWxlPSJmb250LXNpemU6IDM2cHg7IGZvbnQtd2VpZ2h0OiA4MDA7IG1hcmdpbjogMDsgY29sb3I6ICNmZmZmZmY7Ij5BbHplIEUgUG9ydGFsPC9oMT4KCQkJCQkJCQkJCQk8aDMgc3R5bGU9ImZvbnQtc2l6ZTogMTZweDsgZm9udC13ZWlnaHQ6IDgwMDsgbWFyZ2luOiAwOyBjb2xvcjogI2ZmZmZmZjsiPkhvxZ8gZ2VsZGluaXouLjwvaDE+CgkJCQkJCQkJCQkJCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICA8L3RkPgogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICA8L3RyPgogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIDwvdGFibGU+CiAgICAgICAgICAgICAgICAgICAgICAgICAgICA8L2Rpdj4KICAKICAgICAgICAgICAgICAgICAgICAgICAgPC90ZD4KICAgICAgICAgICAgICAgICAgICA8L3RyPgogICAgICAgICAgICAgICAgICAgIDx0cj4KICAgICAgICAgICAgICAgICAgICAgICAgPHRkIGFsaWduPSJjZW50ZXIiIHN0eWxlPSJwYWRkaW5nOiAzNXB4IDM1cHggMjBweCAzNXB4OyBiYWNrZ3JvdW5kLWNvbG9yOiAjZmZmZmZmOyIgYmdjb2xvcj0iI2ZmZmZmZiI+CiAgICAgICAgICAgICAgICAgICAgICAgICAgICA8dGFibGUgYWxpZ249ImNlbnRlciIgYm9yZGVyPSIwIiBjZWxscGFkZGluZz0iMCIgY2VsbHNwYWNpbmc9IjAiIHdpZHRoPSIxMDAlIiBzdHlsZT0ibWF4LXdpZHRoOjYwMHB4OyI+CiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgPHRyPgogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICA8dGQgYWxpZ249ImNlbnRlciIgc3R5bGU9ImZvbnQtZmFtaWx5OiBPcGVuIFNhbnMsIEhlbHZldGljYSwgQXJpYWwsIHNhbnMtc2VyaWY7IGZvbnQtc2l6ZTogMTZweDsgZm9udC13ZWlnaHQ6IDQwMDsgbGluZS1oZWlnaHQ6IDI0cHg7IHBhZGRpbmctdG9wOiAyNXB4OyI+IDxpbWcgc3JjPSJodHRwczovL2ltZy5pY29uczguY29tL2NhcmJvbi1jb3B5LzEwMC8wMDAwMDAvY2hlY2tlZC1jaGVja2JveC5wbmciIHdpZHRoPSIxMjUiIGhlaWdodD0iMTIwIiBzdHlsZT0iZGlzcGxheTogYmxvY2s7IGJvcmRlcjogMHB4OyIgLz48YnI+CiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICA8aDIgc3R5bGU9ImZvbnQtc2l6ZTogMzBweDsgZm9udC13ZWlnaHQ6IDgwMDsgbGluZS1oZWlnaHQ6IDM2cHg7IGNvbG9yOiAjMzMzMzMzOyBtYXJnaW46IDA7Ij4gS2F5xLF0IGnFn2xlbWluaSB0YW1hbWxhZMSxbsSxei4gPC9oMj4KICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgPC90ZD4KICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICA8L3RyPgogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIDx0cj4KICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgPHRkIGFsaWduPSJsZWZ0IiBzdHlsZT0iZm9udC1mYW1pbHk6IE9wZW4gU2FucywgSGVsdmV0aWNhLCBBcmlhbCwgc2Fucy1zZXJpZjsgZm9udC1zaXplOiAxNnB4OyBmb250LXdlaWdodDogNDAwOyBsaW5lLWhlaWdodDogMjRweDsgcGFkZGluZy10b3A6IDEwcHg7Ij4KICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIDxwIHN0eWxlPSJmb250LXNpemU6IDE2cHg7IGZvbnQtd2VpZ2h0OiA0MDA7IGxpbmUtaGVpZ2h0OiAyNHB4OyBjb2xvcjogIzc3Nzc3NzsiPiBBxZ9hxJ/EsWRha2kgS3VsbGFuxLFjxLEgYWTEsSB2ZSDFn2lmcmUgaWxlIGdpcmnFnyB5YXDEsXAgcG9ydGFsxLFtxLF6xLEga3VsbGFubWF5YSBiYcWfbGF5YWJpbGlyc2luaXouIDwvcD4KICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgPC90ZD4KICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICA8L3RyPgogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIDx0cj4KICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgPHRkIGFsaWduPSJsZWZ0IiBzdHlsZT0icGFkZGluZy10b3A6IDIwcHg7Ij4KICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIDx0YWJsZSBjZWxsc3BhY2luZz0iMCIgY2VsbHBhZGRpbmc9IjAiIGJvcmRlcj0iMCIgd2lkdGg9IjEwMCUiPgoJCQkJCQkJCQkJICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICA8dHI+CiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIDx0ZCB3aWR0aD0iNzUlIiBhbGlnbj0ibGVmdCIgc3R5bGU9ImZvbnQtZmFtaWx5OiBPcGVuIFNhbnMsIEhlbHZldGljYSwgQXJpYWwsIHNhbnMtc2VyaWY7IGZvbnQtc2l6ZTogMTZweDsgZm9udC13ZWlnaHQ6IDQwMDsgbGluZS1oZWlnaHQ6IDI0cHg7IHBhZGRpbmc6IDE1cHggMTBweCA1cHggMTBweDsiPiBLdWxsYW7EsWPEsSBBZMSxbsSxeiA8L3RkPgogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICA8dGQgd2lkdGg9IjI1JSIgYWxpZ249ImxlZnQiIHN0eWxlPSJmb250LWZhbWlseTogT3BlbiBTYW5zLCBIZWx2ZXRpY2EsIEFyaWFsLCBzYW5zLXNlcmlmOyBmb250LXNpemU6IDE2cHg7IGZvbnQtd2VpZ2h0OiA0MDA7IGxpbmUtaGVpZ2h0OiAyNHB4OyBwYWRkaW5nOiAxNXB4IDEwcHggNXB4IDEwcHg7Ij4gxZ5pZnJlbml6PC90ZD4KICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICA8L3RyPgogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIDx0cj4KICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgPHRkIHdpZHRoPSI3NSUiIGFsaWduPSJsZWZ0IiBiZ2NvbG9yPSIjZWVlZWVlIiBzdHlsZT0iZm9udC1mYW1pbHk6IE9wZW4gU2FucywgSGVsdmV0aWNhLCBBcmlhbCwgc2Fucy1zZXJpZjsgZm9udC1zaXplOiAxNnB4OyBmb250LXdlaWdodDogODAwOyBsaW5lLWhlaWdodDogMjRweDsgcGFkZGluZzogMTBweDsiPiBbVXNlck5hbWVdIDwvdGQ+CiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIDx0ZCB3aWR0aD0iMjUlIiBhbGlnbj0ibGVmdCIgYmdjb2xvcj0iI2VlZWVlZSIgc3R5bGU9ImZvbnQtZmFtaWx5OiBPcGVuIFNhbnMsIEhlbHZldGljYSwgQXJpYWwsIHNhbnMtc2VyaWY7IGZvbnQtc2l6ZTogMTZweDsgZm9udC13ZWlnaHQ6IDgwMDsgbGluZS1oZWlnaHQ6IDI0cHg7IHBhZGRpbmc6IDEwcHg7Ij4gW1Bhc3N3b3JkXSA8L3RkPgogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIDwvdHI+CiAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIDwvdGFibGU+CiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIDwvdGQ+CiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgPC90cj4KIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgPC90YWJsZT4KICAgICAgICAgICAgICAgICAgICAgICAgPC90ZD4KICAgICAgICAgICAgICAgICAgICA8L3RyPgogCiAgICAgICAgICAgICAgICAgICAgPHRyPgogICAgICAgICAgICAgICAgICAgICAgICA8dGQgYWxpZ249ImNlbnRlciIgc3R5bGU9IiBwYWRkaW5nOiAzNXB4OyBiYWNrZ3JvdW5kLWNvbG9yOiAjZmY3MzYxOyIgYmdjb2xvcj0iIzFiOWJhMyI+CiAgICAgICAgICAgICAgICAgICAgICAgICAgICA8dGFibGUgYWxpZ249ImNlbnRlciIgYm9yZGVyPSIwIiBjZWxscGFkZGluZz0iMCIgY2VsbHNwYWNpbmc9IjAiIHdpZHRoPSIxMDAlIiBzdHlsZT0ibWF4LXdpZHRoOjYwMHB4OyI+CiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgPHRyPgogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICA8dGQgYWxpZ249ImNlbnRlciIgc3R5bGU9ImZvbnQtZmFtaWx5OiBPcGVuIFNhbnMsIEhlbHZldGljYSwgQXJpYWwsIHNhbnMtc2VyaWY7IGZvbnQtc2l6ZTogMTZweDsgZm9udC13ZWlnaHQ6IDQwMDsgbGluZS1oZWlnaHQ6IDI0cHg7IHBhZGRpbmctdG9wOiAyNXB4OyI+CiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICA8aDIgc3R5bGU9ImZvbnQtc2l6ZTogMjRweDsgZm9udC13ZWlnaHQ6IDgwMDsgbGluZS1oZWlnaHQ6IDMwcHg7IGNvbG9yOiAjZmZmZmZmOyBtYXJnaW46IDA7Ij4gSGVtZW4gR2lyacWfIFlhcMSxbiA8L2gyPgogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICA8L3RkPgogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIDwvdHI+CiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgPHRyPgogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICA8dGQgYWxpZ249ImNlbnRlciIgc3R5bGU9InBhZGRpbmc6IDI1cHggMCAxNXB4IDA7Ij4KICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIDx0YWJsZSBib3JkZXI9IjAiIGNlbGxzcGFjaW5nPSIwIiBjZWxscGFkZGluZz0iMCI+CiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgPHRyPgogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICA8dGQgYWxpZ249ImNlbnRlciIgc3R5bGU9ImJvcmRlci1yYWRpdXM6IDVweDsiIGJnY29sb3I9IiM2NmIzYjciPiA8YSBocmVmPSJbTGlua10iIHRhcmdldD0iX2JsYW5rIiBzdHlsZT0iZm9udC1zaXplOiAxOHB4OyBmb250LWZhbWlseTogT3BlbiBTYW5zLCBIZWx2ZXRpY2EsIEFyaWFsLCBzYW5zLXNlcmlmOyBjb2xvcjogI2ZmZmZmZjsgdGV4dC1kZWNvcmF0aW9uOiBub25lOyBib3JkZXItcmFkaXVzOiA1cHg7IGJhY2tncm91bmQtY29sb3I6ICNGNDQzMzY7IHBhZGRpbmc6IDE1cHggMzBweDsgYm9yZGVyOiAxcHggc29saWQgI0Y0NDMzNjsgZGlzcGxheTogYmxvY2s7Ij5BbHplIEUgUG9ydGFsPC9hPiA8L3RkPgogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIDwvdHI+CiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICA8L3RhYmxlPgogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICA8L3RkPgogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIDwvdHI+CiAgICAgICAgICAgICAgICAgICAgICAgICAgICA8L3RhYmxlPgogICAgICAgICAgICAgICAgICAgICAgICA8L3RkPgogICAgICAgICAgICAgICAgICAgIDwvdHI+CiAgICAgICAgICAgICAgICAgICAgPHRyPgogICAgICAgICAgICAgICAgICAgICAgICA8dGQgYWxpZ249ImNlbnRlciIgc3R5bGU9InBhZGRpbmc6IDM1cHg7IGJhY2tncm91bmQtY29sb3I6ICNmZmZmZmY7IiBiZ2NvbG9yPSIjZmZmZmZmIj4KICAgICAgICAgICAgICAgICAgICAgICAgICAgIDx0YWJsZSBhbGlnbj0iY2VudGVyIiBib3JkZXI9IjAiIGNlbGxwYWRkaW5nPSIwIiBjZWxsc3BhY2luZz0iMCIgd2lkdGg9IjEwMCUiIHN0eWxlPSJtYXgtd2lkdGg6NjAwcHg7Ij4KICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIDx0cj4KICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgPHRkIGFsaWduPSJjZW50ZXIiIHN0eWxlPSJmb250LWZhbWlseTogT3BlbiBTYW5zLCBIZWx2ZXRpY2EsIEFyaWFsLCBzYW5zLXNlcmlmOyBmb250LXNpemU6IDE0cHg7IGZvbnQtd2VpZ2h0OiA0MDA7IGxpbmUtaGVpZ2h0OiAyNHB4OyBwYWRkaW5nOiA1cHggMCAxMHB4IDA7Ij4KICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIDxwIHN0eWxlPSJmb250LXNpemU6IDE0cHg7IGZvbnQtd2VpZ2h0OiA4MDA7IGxpbmUtaGVpZ2h0OiAxOHB4OyBjb2xvcjogIzMzMzMzMzsiPiBhbHpldGVrbm9sb2ppLmNvbSA8L3A+CiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIDwvdGQ+CiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgPC90cj4KICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICA8dHI+CiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIDx0ZCBhbGlnbj0ibGVmdCIgc3R5bGU9ImZvbnQtZmFtaWx5OiBPcGVuIFNhbnMsIEhlbHZldGljYSwgQXJpYWwsIHNhbnMtc2VyaWY7IGZvbnQtc2l6ZTogMTRweDsgZm9udC13ZWlnaHQ6IDQwMDsgbGluZS1oZWlnaHQ6IDI0cHg7Ij4KICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIDxwIHN0eWxlPSJmb250LXNpemU6IDE0cHg7IGZvbnQtd2VpZ2h0OiA0MDA7IGxpbmUtaGVpZ2h0OiAyMHB4OyBjb2xvcjogIzc3Nzc3NzsiPiBMw7x0ZmVuIGJ1IG1haWxpIGNldmFwbGFtYXnEsW7EsXouIDwvcD4KICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgPC90ZD4KICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICA8L3RyPgogICAgICAgICAgICAgICAgICAgICAgICAgICAgPC90YWJsZT4KICAgICAgICAgICAgICAgICAgICAgICAgPC90ZD4KICAgICAgICAgICAgICAgICAgICA8L3RyPgogICAgICAgICAgICAgICAgPC90YWJsZT4KICAgICAgICAgICAgPC90ZD4KICAgICAgICA8L3RyPgogICAgPC90YWJsZT4KPC9ib2R5PgoKPC9odG1sPg==";


                case "ResetPassword":

                    return "PCFET0NUWVBFIGh0bWw+Cgo8aHRtbCBsYW5nPSJlbiIgeG1sbnM6bz0idXJuOnNjaGVtYXMtbWljcm9zb2Z0LWNvbTpvZmZpY2U6b2ZmaWNlIiB4bWxuczp2PSJ1cm46c2NoZW1hcy1taWNyb3NvZnQtY29tOnZtbCI+CjxoZWFkPgo8dGl0bGU+PC90aXRsZT4KPG1ldGEgY2hhcnNldD0idXRmLTgiLz4KPG1ldGEgY29udGVudD0id2lkdGg9ZGV2aWNlLXdpZHRoLCBpbml0aWFsLXNjYWxlPTEuMCIgbmFtZT0idmlld3BvcnQiLz4KPCEtLVtpZiBtc29dPjx4bWw+PG86T2ZmaWNlRG9jdW1lbnRTZXR0aW5ncz48bzpQaXhlbHNQZXJJbmNoPjk2PC9vOlBpeGVsc1BlckluY2g+PG86QWxsb3dQTkcvPjwvbzpPZmZpY2VEb2N1bWVudFNldHRpbmdzPjwveG1sPjwhW2VuZGlmXS0tPgo8IS0tW2lmICFtc29dPjwhLS0+CjxsaW5rIGhyZWY9Imh0dHBzOi8vZm9udHMuZ29vZ2xlYXBpcy5jb20vY3NzP2ZhbWlseT1CaXR0ZXIiIHJlbD0ic3R5bGVzaGVldCIgdHlwZT0idGV4dC9jc3MiLz4KPGxpbmsgaHJlZj0iaHR0cHM6Ly9mb250cy5nb29nbGVhcGlzLmNvbS9jc3M/ZmFtaWx5PUNvcm1vcmFudCtHYXJhbW9uZCIgcmVsPSJzdHlsZXNoZWV0IiB0eXBlPSJ0ZXh0L2NzcyIvPgo8bGluayBocmVmPSJodHRwczovL2ZvbnRzLmdvb2dsZWFwaXMuY29tL2Nzcz9mYW1pbHk9Q2FiaW4iIHJlbD0ic3R5bGVzaGVldCIgdHlwZT0idGV4dC9jc3MiLz4KPGxpbmsgaHJlZj0iaHR0cHM6Ly9mb250cy5nb29nbGVhcGlzLmNvbS9jc3M/ZmFtaWx5PURyb2lkK1NlcmlmIiByZWw9InN0eWxlc2hlZXQiIHR5cGU9InRleHQvY3NzIi8+CjxsaW5rIGhyZWY9Imh0dHBzOi8vZm9udHMuZ29vZ2xlYXBpcy5jb20vY3NzP2ZhbWlseT1Nb250c2VycmF0IiByZWw9InN0eWxlc2hlZXQiIHR5cGU9InRleHQvY3NzIi8+CjxsaW5rIGhyZWY9Imh0dHBzOi8vZm9udHMuZ29vZ2xlYXBpcy5jb20vY3NzP2ZhbWlseT1Sb2JvdG8rU2xhYiIgcmVsPSJzdHlsZXNoZWV0IiB0eXBlPSJ0ZXh0L2NzcyIvPgo8IS0tPCFbZW5kaWZdLS0+CjxzdHlsZT4KCQkqIHsKCQkJYm94LXNpemluZzogYm9yZGVyLWJveDsKCQl9CgoJCWJvZHkgewoJCQltYXJnaW46IDA7CgkJCXBhZGRpbmc6IDA7CgkJfQoKCQkvKnRoLmNvbHVtbnsKCXBhZGRpbmc6MAp9Ki8KCgkJYVt4LWFwcGxlLWRhdGEtZGV0ZWN0b3JzXSB7CgkJCWNvbG9yOiBpbmhlcml0ICFpbXBvcnRhbnQ7CgkJCXRleHQtZGVjb3JhdGlvbjogaW5oZXJpdCAhaW1wb3J0YW50OwoJCX0KCgkJI01lc3NhZ2VWaWV3Qm9keSBhIHsKCQkJY29sb3I6IGluaGVyaXQ7CgkJCXRleHQtZGVjb3JhdGlvbjogbm9uZTsKCQl9CgoJCXAgewoJCQlsaW5lLWhlaWdodDogaW5oZXJpdAoJCX0KCgkJQG1lZGlhIChtYXgtd2lkdGg6NjcwcHgpIHsKCQkJLmljb25zLWlubmVyIHsKCQkJCXRleHQtYWxpZ246IGNlbnRlcjsKCQkJfQoKCQkJLmljb25zLWlubmVyIHRkIHsKCQkJCW1hcmdpbjogMCBhdXRvOwoJCQl9CgoJCQkucm93LWNvbnRlbnQgewoJCQkJd2lkdGg6IDEwMCUgIWltcG9ydGFudDsKCQkJfQoKCQkJLnN0YWNrIC5jb2x1bW4gewoJCQkJd2lkdGg6IDEwMCU7CgkJCQlkaXNwbGF5OiBibG9jazsKCQkJfQoJCX0KCTwvc3R5bGU+CjwvaGVhZD4KPGJvZHkgc3R5bGU9ImJhY2tncm91bmQtY29sb3I6ICM4NWE0Y2Q7IG1hcmdpbjogMDsgcGFkZGluZzogMDsgLXdlYmtpdC10ZXh0LXNpemUtYWRqdXN0OiBub25lOyB0ZXh0LXNpemUtYWRqdXN0OiBub25lOyI+Cjx0YWJsZSBib3JkZXI9IjAiIGNlbGxwYWRkaW5nPSIwIiBjZWxsc3BhY2luZz0iMCIgY2xhc3M9Im5sLWNvbnRhaW5lciIgcm9sZT0icHJlc2VudGF0aW9uIiBzdHlsZT0ibXNvLXRhYmxlLWxzcGFjZTogMHB0OyBtc28tdGFibGUtcnNwYWNlOiAwcHQ7IGJhY2tncm91bmQtY29sb3I6ICM4NWE0Y2Q7IiB3aWR0aD0iMTAwJSI+Cjx0Ym9keT4KPHRyPgo8dGQ+Cjx0YWJsZSBhbGlnbj0iY2VudGVyIiBib3JkZXI9IjAiIGNlbGxwYWRkaW5nPSIwIiBjZWxsc3BhY2luZz0iMCIgY2xhc3M9InJvdyByb3ctMSIgcm9sZT0icHJlc2VudGF0aW9uIiBzdHlsZT0ibXNvLXRhYmxlLWxzcGFjZTogMHB0OyBtc28tdGFibGUtcnNwYWNlOiAwcHQ7IiB3aWR0aD0iMTAwJSI+Cjx0Ym9keT4KPHRyPgo8dGQ+Cjx0YWJsZSBhbGlnbj0iY2VudGVyIiBib3JkZXI9IjAiIGNlbGxwYWRkaW5nPSIwIiBjZWxsc3BhY2luZz0iMCIgY2xhc3M9InJvdy1jb250ZW50IHN0YWNrIiByb2xlPSJwcmVzZW50YXRpb24iIHN0eWxlPSJtc28tdGFibGUtbHNwYWNlOiAwcHQ7IG1zby10YWJsZS1yc3BhY2U6IDBwdDsgY29sb3I6ICMwMDAwMDA7IiB3aWR0aD0iNjUwIj4KPHRib2R5Pgo8dHI+Cjx0ZCBjbGFzcz0iY29sdW1uIiBzdHlsZT0ibXNvLXRhYmxlLWxzcGFjZTogMHB0OyBtc28tdGFibGUtcnNwYWNlOiAwcHQ7IGZvbnQtd2VpZ2h0OiA0MDA7IHRleHQtYWxpZ246IGxlZnQ7IHZlcnRpY2FsLWFsaWduOiB0b3A7IHBhZGRpbmctdG9wOiA1cHg7IHBhZGRpbmctYm90dG9tOiA1cHg7IGJvcmRlci10b3A6IDBweDsgYm9yZGVyLXJpZ2h0OiAwcHg7IGJvcmRlci1ib3R0b206IDBweDsgYm9yZGVyLWxlZnQ6IDBweDsiIHdpZHRoPSIxMDAlIj4KPHRhYmxlIGJvcmRlcj0iMCIgY2VsbHBhZGRpbmc9IjAiIGNlbGxzcGFjaW5nPSIwIiBjbGFzcz0iaGVhZGluZ19ibG9jayIgcm9sZT0icHJlc2VudGF0aW9uIiBzdHlsZT0ibXNvLXRhYmxlLWxzcGFjZTogMHB0OyBtc28tdGFibGUtcnNwYWNlOiAwcHQ7IiB3aWR0aD0iMTAwJSI+Cjx0cj4KPHRkIHN0eWxlPSJwYWRkaW5nLWJvdHRvbToxMHB4O3RleHQtYWxpZ246Y2VudGVyO3dpZHRoOjEwMCU7cGFkZGluZy10b3A6NjBweDsiPgo8aDEgc3R5bGU9Im1hcmdpbjogMDsgY29sb3I6ICNmZmZmZmY7IGRpcmVjdGlvbjogbHRyOyBmb250LWZhbWlseTogJ1JvYm90byBTbGFiJywgQXJpYWwsICdIZWx2ZXRpY2EgTmV1ZScsIEhlbHZldGljYSwgc2Fucy1zZXJpZjsgZm9udC1zaXplOiAzMHB4OyBmb250LXdlaWdodDogbm9ybWFsOyBsZXR0ZXItc3BhY2luZzogMnB4OyBsaW5lLWhlaWdodDogMTIwJTsgdGV4dC1hbGlnbjogY2VudGVyOyBtYXJnaW4tdG9wOiAwOyBtYXJnaW4tYm90dG9tOiAwOyI+PHN0cm9uZz5BbHplIEUgUG9ydGFsIHwgxZ5pZnJlIFllbmlsZW1lPC9zdHJvbmc+PC9oMT4KPC90ZD4KPC90cj4KPHRyPgo8dGQgc3R5bGU9InBhZGRpbmctYm90dG9tOjEwcHg7dGV4dC1hbGlnbjpjZW50ZXI7d2lkdGg6MTAwJTtwYWRkaW5nLXRvcDo2MHB4OyI+CjxoMSBzdHlsZT0ibWFyZ2luOiAwOyBjb2xvcjogI2ZmZmZmZjsgZGlyZWN0aW9uOiBsdHI7IGZvbnQtZmFtaWx5OiAnUm9ib3RvIFNsYWInLCBBcmlhbCwgJ0hlbHZldGljYSBOZXVlJywgSGVsdmV0aWNhLCBzYW5zLXNlcmlmOyBmb250LXNpemU6IDMwcHg7IGZvbnQtd2VpZ2h0OiBub3JtYWw7IGxldHRlci1zcGFjaW5nOiAycHg7IGxpbmUtaGVpZ2h0OiAxMjAlOyB0ZXh0LWFsaWduOiBjZW50ZXI7IG1hcmdpbi10b3A6IDA7IG1hcmdpbi1ib3R0b206IDA7Ij48c3Ryb25nPsWeaWZyZW5pemkgbWkgVW51dHR1bnV6ID88L3N0cm9uZz48L2gxPgo8L3RkPgo8L3RyPgo8L3RhYmxlPgo8dGFibGUgYm9yZGVyPSIwIiBjZWxscGFkZGluZz0iMCIgY2VsbHNwYWNpbmc9IjAiIGNsYXNzPSJpbWFnZV9ibG9jayIgcm9sZT0icHJlc2VudGF0aW9uIiBzdHlsZT0ibXNvLXRhYmxlLWxzcGFjZTogMHB0OyBtc28tdGFibGUtcnNwYWNlOiAwcHQ7IiB3aWR0aD0iMTAwJSI+Cjx0cj4KPHRkIHN0eWxlPSJ3aWR0aDoxMDAlO3BhZGRpbmctcmlnaHQ6MHB4O3BhZGRpbmctbGVmdDowcHg7Ij4KPGRpdiBhbGlnbj0iY2VudGVyIiBzdHlsZT0ibGluZS1oZWlnaHQ6MTBweCI+PGltZyBhbHQ9Ildyb25nIFBhc3N3b3JkIEFuaW1hdGlvbiIgc3JjPSJodHRwczovL3d3dy5hbHplYmFja3VwLmNvbS9HSUZfcGFzc3dvcmQuZ2lmIiBzdHlsZT0iZGlzcGxheTogYmxvY2s7IGhlaWdodDogYXV0bzsgYm9yZGVyOiAwOyB3aWR0aDogNTAwcHg7IG1heC13aWR0aDogMTAwJTsiIHRpdGxlPSJXcm9uZyBQYXNzd29yZCBBbmltYXRpb24iIHdpZHRoPSI1MDAiLz48L2Rpdj4KPC90ZD4KPC90cj4KPC90YWJsZT4KPHRhYmxlIGJvcmRlcj0iMCIgY2VsbHBhZGRpbmc9IjAiIGNlbGxzcGFjaW5nPSIwIiBjbGFzcz0idGV4dF9ibG9jayIgcm9sZT0icHJlc2VudGF0aW9uIiBzdHlsZT0ibXNvLXRhYmxlLWxzcGFjZTogMHB0OyBtc28tdGFibGUtcnNwYWNlOiAwcHQ7IHdvcmQtYnJlYWs6IGJyZWFrLXdvcmQ7IiB3aWR0aD0iMTAwJSI+Cjx0cj4KPHRkIHN0eWxlPSJwYWRkaW5nLWJvdHRvbTo1cHg7cGFkZGluZy1sZWZ0OjEwcHg7cGFkZGluZy1yaWdodDoxMHB4O3BhZGRpbmctdG9wOjI1cHg7Ij4KPGRpdiBzdHlsZT0iZm9udC1mYW1pbHk6IHNhbnMtc2VyaWYiPgo8ZGl2IHN0eWxlPSJmb250LXNpemU6IDE0cHg7IG1zby1saW5lLWhlaWdodC1hbHQ6IDE2LjhweDsgY29sb3I6ICMzZjRkNzU7IGxpbmUtaGVpZ2h0OiAxLjI7IGZvbnQtZmFtaWx5OiBSb2JvdG8gU2xhYiwgQXJpYWwsIEhlbHZldGljYSBOZXVlLCBIZWx2ZXRpY2EsIHNhbnMtc2VyaWY7Ij4KPHAgc3R5bGU9Im1hcmdpbjogMDsgZm9udC1zaXplOiAxNHB4OyB0ZXh0LWFsaWduOiBjZW50ZXI7Ij48c3BhbiBzdHlsZT0iZm9udC1zaXplOjIwcHg7Ij5TaXppbiBpw6dpbiDFnmlmcmVuaXppIFPEsWbEsXJsYWTEsWsuPC9zcGFuPjwvcD4KPC9kaXY+CjwvZGl2Pgo8L3RkPgo8L3RyPgo8L3RhYmxlPgo8dGFibGUgYm9yZGVyPSIwIiBjZWxscGFkZGluZz0iMCIgY2VsbHNwYWNpbmc9IjAiIGNsYXNzPSJ0ZXh0X2Jsb2NrIiByb2xlPSJwcmVzZW50YXRpb24iIHN0eWxlPSJtc28tdGFibGUtbHNwYWNlOiAwcHQ7IG1zby10YWJsZS1yc3BhY2U6IDBwdDsgd29yZC1icmVhazogYnJlYWstd29yZDsiIHdpZHRoPSIxMDAlIj4KPHRyPgo8dGQgc3R5bGU9InBhZGRpbmctYm90dG9tOjVweDtwYWRkaW5nLWxlZnQ6MTBweDtwYWRkaW5nLXJpZ2h0OjEwcHg7cGFkZGluZy10b3A6NXB4OyI+CjxkaXYgc3R5bGU9ImZvbnQtZmFtaWx5OiBzYW5zLXNlcmlmIj4KPGRpdiBzdHlsZT0iZm9udC1zaXplOiAxNHB4OyBtc28tbGluZS1oZWlnaHQtYWx0OiAxNi44cHg7IGNvbG9yOiAjM2Y0ZDc1OyBsaW5lLWhlaWdodDogMS4yOyBmb250LWZhbWlseTogUm9ib3RvIFNsYWIsIEFyaWFsLCBIZWx2ZXRpY2EgTmV1ZSwgSGVsdmV0aWNhLCBzYW5zLXNlcmlmOyI+CjxwIHN0eWxlPSJtYXJnaW46IDA7IGZvbnQtc2l6ZTogMTRweDsgdGV4dC1hbGlnbjogY2VudGVyOyI+PHNwYW4gc3R5bGU9ImZvbnQtc2l6ZToyMnB4OyI+QcWfYcSfxLFkYWtpIHllbmkgxZ9pZnJlbml6IGlsZSBnaXJpxZ8geWFwYWJpbGlyc2luaXouPC9zcGFuPjwvcD4KPC9kaXY+CjwvZGl2Pgo8L3RkPgo8L3RyPgo8L3RhYmxlPgo8dGFibGUgYm9yZGVyPSIwIiBjZWxscGFkZGluZz0iMCIgY2VsbHNwYWNpbmc9IjAiIGNsYXNzPSJidXR0b25fYmxvY2siIHJvbGU9InByZXNlbnRhdGlvbiIgc3R5bGU9Im1zby10YWJsZS1sc3BhY2U6IDBwdDsgbXNvLXRhYmxlLXJzcGFjZTogMHB0OyIgd2lkdGg9IjEwMCUiPgo8dHI+Cjx0ZCBzdHlsZT0icGFkZGluZy1ib3R0b206MTBweDtwYWRkaW5nLWxlZnQ6MTBweDtwYWRkaW5nLXJpZ2h0OjEwcHg7cGFkZGluZy10b3A6MzBweDt0ZXh0LWFsaWduOmNlbnRlcjsiPgo8ZGl2IGFsaWduPSJjZW50ZXIiPgo8IS0tW2lmIG1zb10+PHY6cm91bmRyZWN0IHhtbG5zOnY9InVybjpzY2hlbWFzLW1pY3Jvc29mdC1jb206dm1sIiB4bWxuczp3PSJ1cm46c2NoZW1hcy1taWNyb3NvZnQtY29tOm9mZmljZTp3b3JkIiAgc3R5bGU9ImhlaWdodDo2MHB4O3dpZHRoOjEwNXB4O3YtdGV4dC1hbmNob3I6bWlkZGxlOyIgYXJjc2l6ZT0iMTclIiBzdHJva2V3ZWlnaHQ9IjEuNXB0IiBzdHJva2Vjb2xvcj0iIzNGNEQ3NSIgZmlsbGNvbG9yPSIjZmZmZmZmIj48dzphbmNob3Jsb2NrLz48djp0ZXh0Ym94IGluc2V0PSIwcHgsMHB4LDBweCwwcHgiPjxjZW50ZXIgc3R5bGU9ImNvbG9yOiMzZjRkNzU7IGZvbnQtZmFtaWx5OkFyaWFsLCBzYW5zLXNlcmlmOyBmb250LXNpemU6MThweCI+PCFbZW5kaWZdLS0+PHNwYW4gc3R5bGU9InRleHQtZGVjb3JhdGlvbjpub25lO2Rpc3BsYXk6aW5saW5lLWJsb2NrO2NvbG9yOiMzZjRkNzU7YmFja2dyb3VuZC1jb2xvcjojZmZmZmZmO2JvcmRlci1yYWRpdXM6MTBweDt3aWR0aDphdXRvO2JvcmRlci10b3A6MnB4IHNvbGlkICMzRjRENzU7Ym9yZGVyLXJpZ2h0OjJweCBzb2xpZCAjM0Y0RDc1O2JvcmRlci1ib3R0b206MnB4IHNvbGlkICMzRjRENzU7Ym9yZGVyLWxlZnQ6MnB4IHNvbGlkICMzRjRENzU7cGFkZGluZy10b3A6MTBweDtwYWRkaW5nLWJvdHRvbToxMHB4O2ZvbnQtZmFtaWx5OlJvYm90byBTbGFiLCBBcmlhbCwgSGVsdmV0aWNhIE5ldWUsIEhlbHZldGljYSwgc2Fucy1zZXJpZjt0ZXh0LWFsaWduOmNlbnRlcjttc28tYm9yZGVyLWFsdDpub25lO3dvcmQtYnJlYWs6a2VlcC1hbGw7IiA+PHNwYW4gc3R5bGU9InBhZGRpbmctbGVmdDoyNXB4O3BhZGRpbmctcmlnaHQ6MjVweDtmb250LXNpemU6MThweDtkaXNwbGF5OmlubGluZS1ibG9jaztsZXR0ZXItc3BhY2luZzpub3JtYWw7Ij48c3BhbiBzdHlsZT0iZm9udC1zaXplOiAxNnB4OyBsaW5lLWhlaWdodDogMjsgd29yZC1icmVhazogYnJlYWstd29yZDsgbXNvLWxpbmUtaGVpZ2h0LWFsdDogMzJweDsiPjxzcGFuIGRhdGEtbWNlLXN0eWxlPSJmb250LXNpemU6IDE4cHg7IGxpbmUtaGVpZ2h0OiAzNnB4OyIgc3R5bGU9ImZvbnQtc2l6ZTogMThweDsgbGluZS1oZWlnaHQ6IDM2cHg7Ij5bUGFzc3dvcmRdPC9zcGFuPjwvc3Bhbj48L3NwYW4+PC9zcGFuPgo8IS0tW2lmIG1zb10+PC9jZW50ZXI+PC92OnRleHRib3g+PC92OnJvdW5kcmVjdD48IVtlbmRpZl0tLT4KPC9kaXY+CjwvdGQ+CjwvdHI+CjwvdGFibGU+Cjx0YWJsZSBib3JkZXI9IjAiIGNlbGxwYWRkaW5nPSIwIiBjZWxsc3BhY2luZz0iMCIgY2xhc3M9InRleHRfYmxvY2siIHJvbGU9InByZXNlbnRhdGlvbiIgc3R5bGU9Im1zby10YWJsZS1sc3BhY2U6IDBwdDsgbXNvLXRhYmxlLXJzcGFjZTogMHB0OyB3b3JkLWJyZWFrOiBicmVhay13b3JkOyIgd2lkdGg9IjEwMCUiPgo8dHI+Cjx0ZCBzdHlsZT0icGFkZGluZy1ib3R0b206NDBweDtwYWRkaW5nLWxlZnQ6MTBweDtwYWRkaW5nLXJpZ2h0OjEwcHg7cGFkZGluZy10b3A6MzBweDsiPgo8ZGl2IHN0eWxlPSJmb250LWZhbWlseTogc2Fucy1zZXJpZiI+CjxkaXYgc3R5bGU9ImZvbnQtc2l6ZTogMTJweDsgbXNvLWxpbmUtaGVpZ2h0LWFsdDogMTQuMzk5OTk5OTk5OTk5OTk5cHg7IGNvbG9yOiAjM2Y0ZDc1OyBsaW5lLWhlaWdodDogMS4yOyBmb250LWZhbWlseTogUm9ib3RvIFNsYWIsIEFyaWFsLCBIZWx2ZXRpY2EgTmV1ZSwgSGVsdmV0aWNhLCBzYW5zLXNlcmlmOyI+CjxwIHN0eWxlPSJtYXJnaW46IDA7IGZvbnQtc2l6ZTogMTJweDsgbXNvLWxpbmUtaGVpZ2h0LWFsdDogMTQuMzk5OTk5OTk5OTk5OTk5cHg7Ij7CoDwvcD4KPC9kaXY+CjwvZGl2Pgo8L3RkPgo8L3RyPgo8L3RhYmxlPgo8L3RkPgo8L3RyPgo8L3Rib2R5Pgo8L3RhYmxlPgo8L3RkPgo8L3RyPgo8L3Rib2R5Pgo8L3RhYmxlPgo8dGFibGUgYWxpZ249ImNlbnRlciIgYm9yZGVyPSIwIiBjZWxscGFkZGluZz0iMCIgY2VsbHNwYWNpbmc9IjAiIGNsYXNzPSJyb3cgcm93LTIiIHJvbGU9InByZXNlbnRhdGlvbiIgc3R5bGU9Im1zby10YWJsZS1sc3BhY2U6IDBwdDsgbXNvLXRhYmxlLXJzcGFjZTogMHB0OyBiYWNrZ3JvdW5kLWNvbG9yOiAjYzRkNmVjOyIgd2lkdGg9IjEwMCUiPgo8dGJvZHk+Cjx0cj4KPHRkPgo8dGFibGUgYWxpZ249ImNlbnRlciIgYm9yZGVyPSIwIiBjZWxscGFkZGluZz0iMCIgY2VsbHNwYWNpbmc9IjAiIGNsYXNzPSJyb3ctY29udGVudCBzdGFjayIgcm9sZT0icHJlc2VudGF0aW9uIiBzdHlsZT0ibXNvLXRhYmxlLWxzcGFjZTogMHB0OyBtc28tdGFibGUtcnNwYWNlOiAwcHQ7IGNvbG9yOiAjMDAwMDAwOyIgd2lkdGg9IjY1MCI+Cjx0Ym9keT4KPHRyPgo8dGQgY2xhc3M9ImNvbHVtbiIgc3R5bGU9Im1zby10YWJsZS1sc3BhY2U6IDBwdDsgbXNvLXRhYmxlLXJzcGFjZTogMHB0OyBmb250LXdlaWdodDogNDAwOyB0ZXh0LWFsaWduOiBsZWZ0OyB2ZXJ0aWNhbC1hbGlnbjogdG9wOyBwYWRkaW5nLXRvcDogMjBweDsgcGFkZGluZy1ib3R0b206IDIwcHg7IGJvcmRlci10b3A6IDBweDsgYm9yZGVyLXJpZ2h0OiAwcHg7IGJvcmRlci1ib3R0b206IDBweDsgYm9yZGVyLWxlZnQ6IDBweDsiIHdpZHRoPSIxMDAlIj4KPHRhYmxlIGJvcmRlcj0iMCIgY2VsbHBhZGRpbmc9IjEwIiBjZWxsc3BhY2luZz0iMCIgY2xhc3M9InRleHRfYmxvY2siIHJvbGU9InByZXNlbnRhdGlvbiIgc3R5bGU9Im1zby10YWJsZS1sc3BhY2U6IDBwdDsgbXNvLXRhYmxlLXJzcGFjZTogMHB0OyB3b3JkLWJyZWFrOiBicmVhay13b3JkOyIgd2lkdGg9IjEwMCUiPgo8dHI+Cjx0ZD4KIAo8L3RkPgo8L3RyPgo8L3RhYmxlPgo8L3RkPgo8L3RyPgo8L3Rib2R5Pgo8L3RhYmxlPgo8L3RkPgo8L3RyPgo8L3Rib2R5Pgo8L3RhYmxlPgo8dGFibGUgYWxpZ249ImNlbnRlciIgYm9yZGVyPSIwIiBjZWxscGFkZGluZz0iMCIgY2VsbHNwYWNpbmc9IjAiIGNsYXNzPSJyb3cgcm93LTMiIHJvbGU9InByZXNlbnRhdGlvbiIgc3R5bGU9Im1zby10YWJsZS1sc3BhY2U6IDBwdDsgbXNvLXRhYmxlLXJzcGFjZTogMHB0OyBiYWNrZ3JvdW5kLWNvbG9yOiAjZjNmNmZlOyIgd2lkdGg9IjEwMCUiPgo8dGJvZHk+Cjx0cj4KPHRkPgo8dGFibGUgYWxpZ249ImNlbnRlciIgYm9yZGVyPSIwIiBjZWxscGFkZGluZz0iMCIgY2VsbHNwYWNpbmc9IjAiIGNsYXNzPSJyb3ctY29udGVudCBzdGFjayIgcm9sZT0icHJlc2VudGF0aW9uIiBzdHlsZT0ibXNvLXRhYmxlLWxzcGFjZTogMHB0OyBtc28tdGFibGUtcnNwYWNlOiAwcHQ7IGNvbG9yOiAjMDAwMDAwOyIgd2lkdGg9IjY1MCI+Cjx0Ym9keT4KPHRyPgo8dGQgY2xhc3M9ImNvbHVtbiIgc3R5bGU9Im1zby10YWJsZS1sc3BhY2U6IDBwdDsgbXNvLXRhYmxlLXJzcGFjZTogMHB0OyBmb250LXdlaWdodDogNDAwOyB0ZXh0LWFsaWduOiBsZWZ0OyB2ZXJ0aWNhbC1hbGlnbjogdG9wOyBwYWRkaW5nLXRvcDogMHB4OyBwYWRkaW5nLWJvdHRvbTogMHB4OyBib3JkZXItdG9wOiAwcHg7IGJvcmRlci1yaWdodDogMHB4OyBib3JkZXItYm90dG9tOiAwcHg7IGJvcmRlci1sZWZ0OiAwcHg7IiB3aWR0aD0iMTAwJSI+CjxkaXYgY2xhc3M9InNwYWNlcl9ibG9jayIgc3R5bGU9ImhlaWdodDo3MHB4O2xpbmUtaGVpZ2h0OjMwcHg7Zm9udC1zaXplOjFweDsiPuKAijwvZGl2Pgo8L3RkPgo8L3RyPgo8L3Rib2R5Pgo8L3RhYmxlPgo8L3RkPgo8L3RyPgo8L3Rib2R5Pgo8L3RhYmxlPgo8dGFibGUgYWxpZ249ImNlbnRlciIgYm9yZGVyPSIwIiBjZWxscGFkZGluZz0iMCIgY2VsbHNwYWNpbmc9IjAiIGNsYXNzPSJyb3cgcm93LTQiIHJvbGU9InByZXNlbnRhdGlvbiIgc3R5bGU9Im1zby10YWJsZS1sc3BhY2U6IDBwdDsgbXNvLXRhYmxlLXJzcGFjZTogMHB0OyIgd2lkdGg9IjEwMCUiPgo8dGJvZHk+Cjx0cj4KPHRkPgo8dGFibGUgYWxpZ249ImNlbnRlciIgYm9yZGVyPSIwIiBjZWxscGFkZGluZz0iMCIgY2VsbHNwYWNpbmc9IjAiIGNsYXNzPSJyb3ctY29udGVudCBzdGFjayIgcm9sZT0icHJlc2VudGF0aW9uIiBzdHlsZT0ibXNvLXRhYmxlLWxzcGFjZTogMHB0OyBtc28tdGFibGUtcnNwYWNlOiAwcHQ7IGNvbG9yOiAjMDAwMDAwOyIgd2lkdGg9IjY1MCI+Cjx0Ym9keT4KPHRyPgo8dGQgY2xhc3M9ImNvbHVtbiIgc3R5bGU9Im1zby10YWJsZS1sc3BhY2U6IDBwdDsgbXNvLXRhYmxlLXJzcGFjZTogMHB0OyBmb250LXdlaWdodDogNDAwOyB0ZXh0LWFsaWduOiBsZWZ0OyB2ZXJ0aWNhbC1hbGlnbjogdG9wOyBwYWRkaW5nLXRvcDogNXB4OyBwYWRkaW5nLWJvdHRvbTogNXB4OyBib3JkZXItdG9wOiAwcHg7IGJvcmRlci1yaWdodDogMHB4OyBib3JkZXItYm90dG9tOiAwcHg7IGJvcmRlci1sZWZ0OiAwcHg7IiB3aWR0aD0iMTAwJSI+Cjx0YWJsZSBib3JkZXI9IjAiIGNlbGxwYWRkaW5nPSIwIiBjZWxsc3BhY2luZz0iMCIgY2xhc3M9Imljb25zX2Jsb2NrIiByb2xlPSJwcmVzZW50YXRpb24iIHN0eWxlPSJtc28tdGFibGUtbHNwYWNlOiAwcHQ7IG1zby10YWJsZS1yc3BhY2U6IDBwdDsiIHdpZHRoPSIxMDAlIj4KPHRyPgo8dGQgc3R5bGU9ImNvbG9yOiM5ZDlkOWQ7Zm9udC1mYW1pbHk6aW5oZXJpdDtmb250LXNpemU6MTVweDtwYWRkaW5nLWJvdHRvbTo1cHg7cGFkZGluZy10b3A6NXB4O3RleHQtYWxpZ246Y2VudGVyOyI+Cjx0YWJsZSBjZWxscGFkZGluZz0iMCIgY2VsbHNwYWNpbmc9IjAiIHJvbGU9InByZXNlbnRhdGlvbiIgc3R5bGU9Im1zby10YWJsZS1sc3BhY2U6IDBwdDsgbXNvLXRhYmxlLXJzcGFjZTogMHB0OyIgd2lkdGg9IjEwMCUiPgogCjwvdGFibGU+CjwvdGQ+CjwvdHI+CjwvdGFibGU+CjwvdGQ+CjwvdHI+CjwvdGFibGU+CjwvdGQ+CjwvdHI+CjwvdGJvZHk+CjwvdGFibGU+CjwvdGQ+CjwvdHI+CjwvdGJvZHk+CjwvdGFibGU+CjwvdGQ+CjwvdHI+CjwvdGJvZHk+CjwvdGFibGU+PCEtLSBFbmQgLS0+CjwvYm9keT4KPC9odG1sPg==";
            }

            return "";
        }
    }
}