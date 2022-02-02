using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElektraReport.Models.ReportModels
{
    public class PosSaleReport
    {
        public DateTime Tarih { get; set; }
        public DateTime ASaati { get; set; }
        public DateTime KSaati { get; set; }
        public int Kisi { get; set; }
        public int Fiscounter { get; set; }
        public string Fisno { get; set; }
        public string Islem { get; set; }
        public string Masano { get; set; }
        public string Garson { get; set; }
        public double IndTutar { get; set; }
        public double Kredi { get; set; }
        public double KKarti { get; set; }
        public double Nakit { get; set; }
        public double Toplam { get; set; }
    }
}
