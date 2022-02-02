﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElektraReport.Models.ReportModels
{
    public class PosNotPayDetailReport
    {
        public DateTime Tarih { get; set; }
        public string StokAdi { get; set; }
        public double Adet { get; set; }
        public double Tutar { get; set; }
        public string CariUnvani { get; set; }
    }
}
