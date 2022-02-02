using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElektraReport.Models.ReportModels
{
    public class PosCreditReport
    {
        public DateTime Tarih { get; set; }
        public int FisCounter { get; set; }
        public string FisNo { get; set; }
        public string Islem { get; set; }
        public string MasaNo { get; set; }
        public string Garson { get; set; }
        public string MusteriAdi { get; set; }
        public double Toplam { get; set; }
    }
}
