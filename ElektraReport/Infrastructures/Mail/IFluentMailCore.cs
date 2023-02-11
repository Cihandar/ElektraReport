using ElektraReport.Models.ResultModels;
using System.Threading.Tasks;

namespace ElektraReport.Infrastructures.Mail
{
    public interface IFluentMailCore
    {
        Task<ResultJson2> Send(string email,string code);
    }
}