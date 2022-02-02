using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElektraReport.Models.ReportModels
{
    public class PosOpenTablesDetails
    {
        public string UrunAdi { get; set; }
        public double Adet { get; set; }
        public double BFiyat { get; set; }
        public double Toplam { get; set; }
        public DateTime Tarih { get; set; }
        public DateTime ASaati { get; set; }
        public double GenelToplam { get; set; }
        public string Garson { get; set; }
    }
}
