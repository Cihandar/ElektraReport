using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElektraReport.Models
{
    public class DepremKayit : BaseModel
    {
        public string  OtelAdi { get; set; }
        public string Odano { get; set; }
        public string TcNo { get; set; }
        public string Adi { get; set; }
        public string Soyadi { get; set; }
        public DateTime GirisTarihi { get; set; }
        public DateTime CikisTarihi { get; set; }
        public DateTime DogumTarihi { get; set; }
        public string GsmNo { get; set; }
        public string GeldigiIl { get; set; }
        public string FormVar { get; set; }
        public string RezervasyonNo { get; set; }
        public string Cinsiyet { get; set; }
        public bool isCheckOut { get; set; }
    }
}
