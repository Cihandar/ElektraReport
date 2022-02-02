using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElektraReport.Models.App
{
    public class PosOrders
    {
        public string Tur { get; set; }
        public int SatirId { get; set; }
        public int FisId { get; set; }
        public string UrunAdi { get; set; }
        public int Adet { get; set; }
        public DateTime SiparisSaati { get; set; }
        public string Garson { get; set; }
        public DateTime MasaAcilisSaati { get; set; }
        public string MasaNo { get; set; }
        public string Aciklama { get; set; }
    }
}
