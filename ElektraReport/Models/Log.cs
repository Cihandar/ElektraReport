using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElektraReport.Models
{
    public class Log : BaseModel
    {
        public DateTime Tarih { get; set; }
        public string  UserId { get; set; }
        public string UserName { get; set; }
        public string ClientIp { get; set; }
        public string Note { get; set; }
    }
}
