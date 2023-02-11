using System.Threading.Tasks;

namespace ElektraReport.Infrastructures.Mail
{
    public interface IFluentMailCore
    {
        Task Send(string email,string code);
    }
}