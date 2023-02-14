﻿using AutoMapper;
using ElektraReport.Interfaces.Mappings;
using ElektraReport.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElektraReport.Applications.DepremKayits.ViewModels
{
    public class VM_DepremKayit : IHaveCustomMapping
    {
        public Guid Id { get; set; }
        public Guid CompanyId { get; set; }
        public string OtelAdi { get; set; }
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
        public string ClientIp { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ModifyDate { get; set; }
        public string ModifyClientIp { get; set; }
        public bool BlackList { get; set; } = false;
        public string BlackListNote { get; set; }
        public void CreateMappings(Profile configuration)
        {
            configuration.CreateMap<VM_DepremKayit, DepremKayit>();
            configuration.CreateMap<DepremKayit, VM_DepremKayit>();
        }

    }
}
