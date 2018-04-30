using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BitirmeProjesi.Models
{
    public class GunlukDurum
    {
        public string ilAdi { get; set; }
        public string ilceAdi { get; set; }
        public int GunlukistNo { get; set; }
        public DateTime InsertDate { get; set; }
        public DateTime TarihGun { get; set; }
        public double lat { get; set; }
        public double lang { get; set; }
        public int oncelik { get; set; }
        public decimal EnyuksekSicaklik { get; set; }
        public decimal EnDusukSicaklik { get; set; }
        public string hadiseKodu { get; set; }
        public DateTime tarih { get; set; }
        public string Resim { get; set; }
        public string Aciklama { get; set; }

    }
}