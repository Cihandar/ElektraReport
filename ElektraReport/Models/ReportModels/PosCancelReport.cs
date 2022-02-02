using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElektraReport.Models.ReportModels
{
    public class PosCancelReport
    {
        public DateTime Tarih { get; set; }
        public string UrunAdi { get; set; }
        public DateTime Saat { get; set; }
        public DateTime SipSaat { get; set; }
        public DateTime Sure { get; set; }
        public double Adet { get; set; }
        public string MasaNo { get; set; }
        public string Tur { get; set; }
        public object MusteriAdi { get; set; }
        public int Fisno { get; set; }
        public string GarsonAdi { get; set; }
        public string Aciklama { get; set; }
        public double Tutar { get; set; }
    }
}
