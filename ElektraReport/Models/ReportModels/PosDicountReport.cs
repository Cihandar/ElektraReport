using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElektraReport.Models.ReportModels
{
    public class PosDicountReport
    {
        public DateTime Tarih { get; set; }
        public int FisCounter { get; set; }
        public string Fisno { get; set; }
        public string Islem { get; set; }
        public string MasaNo { get; set; }
        public string Garson { get; set; }
        public double IndTutar { get; set; }
    }
}
