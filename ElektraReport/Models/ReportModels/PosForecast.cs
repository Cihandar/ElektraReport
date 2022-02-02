using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElektraReport.Models.ReportModels
{
    public class PosForecast
    {
        public DateTime Tarih { get; set; }
        public double NakitToplam { get; set; }
        public int NakitAdet { get; set; }
        public double KkToplam { get; set; }
        public int KkAdet { get; set; }
        public double OToplam { get; set; }
        public int OAdet { get; set; }
        public int NKisi { get; set; }
        public int KKisi { get; set; }
        public int OKisi { get; set; }
        public double CariTutar { get; set; }
        public int CariAdet { get; set; }
        public double AcikMasaToplam { get; set; }
        public int AcikMasaAdet { get; set; }
        public int AcikMasaKisi { get; set; }
        public int ToplamAdet { get; set; }
        public int ToplamKisi { get; set; }
        public double? NkCariTahsilat{ get; set; }
        public double? KkCariTahsilat { get; set; }
        public double SatisToplam { get; set; }
        public double KasaToplam { get; set; }

    }
}
