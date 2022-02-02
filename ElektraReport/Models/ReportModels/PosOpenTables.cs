using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElektraReport.Models.ReportModels
{
    public class PosOpenTables
    {
        public int Id { get; set; }
        public DateTime Tarih { get; set; }
        public string MasaNo { get; set; }
        public DateTime ASaati { get; set; }
        public DateTime KSaati { get; set; }
        public string Garson { get; set; }
        public double Toplam { get; set; }
    }
}
