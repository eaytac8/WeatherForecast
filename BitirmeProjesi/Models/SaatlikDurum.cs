using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BitirmeProjesi.Models
{
    public class SaatlikDurum
    {
        public string ilAdi { get; set; }
        public string ilceAdi { get; set; }
        public double lat { get; set; }
        public double lang { get; set; }
        public int oncelik { get; set; }
        public string SaatlikistNo { get; set; }
        public DateTime InsertDate { get; set; }
        public DateTime tarih { get; set; }
        public string Hadise { get; set; }
        public decimal Sicaklik { get; set; }
        public decimal HissedilenSicaklik { get; set; }
        public int Nem { get; set; }
        public int RuzgarYonu { get; set; }
        public int RuzgarHizi { get; set; }
        public int MaxRuzgarHizi { get; set; }
        public string Resim { get; set; }
    }
}