using ElektraReport.Models.ResultModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElektraReport.Infrastructures.Mail
{
    public interface ISendEmail
    {
        Task<ResultJson2> Send(string to, string subject, string message, string name, string Password, string template);
    }
}