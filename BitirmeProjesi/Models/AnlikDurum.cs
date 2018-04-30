using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BitirmeProjesi.Models
{
    public class AnlikDurum
    {
        public string ilAdi { get; set; }
        public string ilceAdi { get; set; }
        public double lat { get; set; }
        public double lang { get; set; }
        public int oncelik { get; set; }
        public int istNo { get; set; }
        public DateTime InsertDate { get; set; }
        public decimal AktuelBasinc { get; set; }
        public decimal DenizSicaklik { get; set; }
        public decimal DenizIndirgenmisBasinc { get; set; }
        public decimal Gorus { get; set; }
        public string HadiseKodu { get; set; }
        public int Kapalilik { get; set; }
        public decimal KarYukseklik { get; set; }
        public decimal Nem { get; set; }
        public string RasatMetar { get; set; }
        public string RasatSinoptik { get; set; }
        public string RasatTaf { get; set; }
        public decimal RuzgarHizi { get; set; }
        public decimal RuzgarYon { get; set; }
        public decimal Sicaklik { get; set; }
        public DateTime VeriZamani { get; set; }
        public decimal Yagis00now { get; set; }
        public decimal Yagis10dk { get; set; }
        public decimal Yagis12Saat { get; set; }
        public decimal Yagis1Saat { get; set; }
        public decimal Yagis24Saat { get; set; }
        public decimal Yagis6Saat { get; set; }
        public DateTime DenizVeriZamani { get; set; }

    }
}