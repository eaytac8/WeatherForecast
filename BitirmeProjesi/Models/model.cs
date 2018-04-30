using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BitirmeProjesi.Models
{
    public class model
    {
        public double lat { get; set; }

        public double lang { get; set; }

        public int oncelik { get; set; }

        public string ilAd { get; set; }

        public string ilceAd { get; set; }

        public decimal EnyuksekSicaklik { get; set; }

        public decimal EnDusukSicaklik { get; set; }

        public string hadiseKodu { get; set; }

        public DateTime tarih { get; set; }
        
        public string Resim { get; set; }

        public int GunlukIstasyon { get; set; }

        public int SonDurumIstasyon { get; set; }

        public string SaatlikIstasyon { get; set; }

        public string Aciklama { get; set; }
    }
}