using BitirmeProjesi.Models;
using BitirmeProjesi.MyDatabase;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BitirmeProjesi.Controllers
{
    public class HomeController : Controller
    {


        // GET: Home
        public ActionResult Anasayfa()
        {

            return View();
        }
        public ActionResult Index()
        {
            DBMysql db = new DBMysql();
            db.OpenConnection();
            List<string> iller = new List<string>();
            List<SelectListItem> Liste_il = new List<SelectListItem>();
            MySqlDataReader reader = db.CommandReader("select * from CityMasterTable");

            while (reader.Read())
            {
                if (reader.GetInt32(2) > 0)
                {
                    iller.Add(reader.GetString(0));
                }
            }
            reader.Close();

            //il Listesini dolduruyoruz..
            int sayac = 1;
            foreach (string item in iller)
            {
                SelectListItem _ilListe = new SelectListItem();
                _ilListe.Text = item;
                _ilListe.Value = sayac.ToString();
                sayac++;
                Liste_il.Add(_ilListe);
            }
            ViewBag.liste = Liste_il;

            db.CloseConnection();
            return View();
        }
        public JsonResult VerileriGetir()
        {
            DBMysql db = new DBMysql();

            List<GunlukDurum> nesneListem = new List<GunlukDurum>();
            GunlukDurum gunlukNesne;
            db.OpenConnection();
            List<SelectListItem> Liste_il = new List<SelectListItem>();
            MySqlDataReader reader = db.CommandReader("select * from CityMasterTable");

            while (reader.Read())
            {
                if (reader.GetInt32(2) > 0)
                {
                    gunlukNesne = new GunlukDurum();
                    gunlukNesne.lat = reader.GetDouble(4);
                    gunlukNesne.lang = reader.GetDouble(5);
                    gunlukNesne.ilAdi = reader.GetString(0);
                    gunlukNesne.GunlukistNo = reader.GetInt32(6);
                    nesneListem.Add(gunlukNesne);
                }
            }
            reader.Close();
            int hours = DateTime.Now.Hour;
            int day = DateTime.Now.Day;
            string h1 = "";
            string h2 = "";
            string dd = "";
            if (hours < 3 || hours >= 15)
            {
                h1 = "14";
                h2 = "16";
                day--;
            }
            else
            {
                h1 = "02";
                h2 = "04";
            }
            if (day < 10)
            {
                dd = "0" + day.ToString();
            }
            else
            {
                dd = day.ToString();
            }
            string date = DateTime.Now.ToString($"yyyy-MM-{dd} {h1}:00:00");
            string date1 = DateTime.Now.ToString($"yyyy-MM-{dd} {h2}:00:00");

            reader = db.CommandReader($"SELECT * FROM DailyForecasts WHERE (InsertDate BETWEEN '{date}' AND '{date1}')");
            while (reader.Read())
            {

                if (Convert.ToInt32(reader.GetInt32(2)) % 100 == 1)
                {
                    int index = nesneListem.FindIndex(a => a.GunlukistNo == reader.GetInt32(2));

                    nesneListem[index].EnDusukSicaklik = reader.GetDecimal(9);
                    nesneListem[index].EnyuksekSicaklik = reader.GetDecimal(14);
                    nesneListem[index].hadiseKodu = reader.GetString(29);
                    nesneListem[index].Resim = reader.GetString(29) + ".png";
                }
            }
            db.CloseConnection();
            return Json(nesneListem, JsonRequestBehavior.AllowGet);
        }
        public List<SaatlikDurum> SaatlikIstek(DBMysql db, MySqlDataReader reader, List<model> Nesneler, string[] veriler, int saat)
        {
            db = new DBMysql();
            db.OpenConnection();
            List<SaatlikDurum> saatlikNesne = new List<SaatlikDurum>();
            SaatlikDurum s_nesne;
            int hourWather = (int.Parse(veriler[2]) - 1) * 3;
            if (hourWather > saat)
            {
                hourWather = (hourWather - saat) / 3;
            }
            else if (hourWather < saat)
            {
                hourWather = (24 - saat + hourWather) / 3;
            }
            else
            {
                hourWather = 0;
            }
            string _saat;
            if (saat < 10)
            {
                _saat = "0" + saat.ToString();
            }
            else
            {
                _saat = saat.ToString();
            }
            string date = DateTime.Now.ToString($"{veriler[3]} {_saat}");
            int indexSayac_ = 0;
            reader = db.CommandReader($"SELECT * FROM HourlyForecasts WHERE tarih1='{date}'");
            while (reader.Read())
            {
                int index = Nesneler.FindIndex(a => a.SaatlikIstasyon == reader.GetString(2));

                s_nesne = new SaatlikDurum();

                s_nesne.ilAdi = Nesneler[index].ilAd;
                s_nesne.lat = Nesneler[index].lat;
                s_nesne.lang = Nesneler[index].lang;
                s_nesne.ilceAdi = Nesneler[index].ilceAd;
                s_nesne.SaatlikistNo = Nesneler[index].SaatlikIstasyon;
                s_nesne.Hadise = reader.GetString(5 + hourWather * 8);
                s_nesne.Sicaklik = reader.GetDecimal(6 + hourWather * 8);
                s_nesne.HissedilenSicaklik = reader.GetDecimal(7 + hourWather * 8);
                s_nesne.Nem = reader.GetInt32(8 + hourWather * 8);
                s_nesne.RuzgarYon = reader.GetInt32(9 + hourWather * 8);
                s_nesne.RuzgarHizi = reader.GetInt32(10 + hourWather * 8);
                s_nesne.MaxRuzgarHizi = reader.GetInt32(11 + hourWather * 8);
                s_nesne.Resim = reader.GetString(5 + hourWather * 8) + ".png";
                if (indexSayac_ == 0 || saatlikNesne[indexSayac_ - 1].SaatlikistNo != s_nesne.SaatlikistNo)
                {
                    saatlikNesne.Add(s_nesne);
                    indexSayac_++;
                }
            }
            db.CloseConnection();
            return saatlikNesne;
        }
        public JsonResult VeriGetir(string[] veriler)
        {
            DBMysql db = new DBMysql();
            db.OpenConnection();
            MySqlDataReader reader;
            List<model> Nesneler = new List<model>();
            model md;
            if (veriler[0] != "il Seçin" && veriler[1] == "2")
            {
                reader = db.CommandReader($"select * from CityMasterTable Where Oncelik='1'");
            }
            else if (veriler[0] != "il Seçin")
            {
                reader = db.CommandReader($"select * from CityMasterTable Where ilAdi='{veriler[0]}'");
            }
            else
            {
                reader = db.CommandReader($"select * from CityMasterTable Where Oncelik='1'");
            }
            while (reader.Read())
            {
                if (reader.GetInt32(6) % 100 != 1 || reader.GetInt32(2) != 0)
                {
                    md = new model();
                    md.lat = reader.GetDouble(4);//enlem
                    md.lang = reader.GetDouble(5);//boylam
                    md.ilAd = reader.GetString(0);//il ad
                    md.ilceAd = reader.GetString(1);//ilce ad
                    md.GunlukIstasyon = reader.GetInt32(6);//Gunlük istasyon no
                    if (!reader.IsDBNull(reader.GetOrdinal("SaatlikTahminIstNo")))
                    {
                        md.SaatlikIstasyon = reader.GetString(7);//Saatlik istasyon no    
                    }
                    if (!reader.IsDBNull(reader.GetOrdinal("SonDurumTahminIstNo")))
                    {
                        md.SonDurumIstasyon = reader.GetInt32(8);//son durum tahmin istasyon
                    }
                    Nesneler.Add(md);

                }
            }
            if (veriler[1] == "1")
            {
                List<GunlukDurum> gunlukNesne = new List<GunlukDurum>();
                GunlukDurum g_nesne;
                reader.Close();
                string date = DateTime.Now.ToString($"{veriler[3]} 02:00:00");
                string date1 = DateTime.Now.ToString($"{veriler[3]} 04:00:00");
                int dayWeather = int.Parse(veriler[2]) - 1;
                if (veriler[0] != "il Seçin")
                {
                    reader = db.CommandReader($"SELECT * FROM DailyForecasts WHERE (InsertDate BETWEEN '{date}' AND '{date1}') AND ilAdi='{veriler[0]}'");
                    while (reader.Read())
                    {
                        int index = Nesneler.FindIndex(a => a.GunlukIstasyon == reader.GetInt32(2));
                        g_nesne = new GunlukDurum();

                        g_nesne.ilAdi = Nesneler[index].ilAd;
                        g_nesne.lat = Nesneler[index].lat;
                        g_nesne.lang = Nesneler[index].lang;
                        g_nesne.ilceAdi = Nesneler[index].ilceAd;
                        g_nesne.GunlukistNo = Nesneler[index].GunlukIstasyon;
                        g_nesne.EnDusukSicaklik = reader.GetDecimal(9 + dayWeather);
                        g_nesne.EnyuksekSicaklik = reader.GetDecimal(14 + dayWeather);
                        g_nesne.hadiseKodu = reader.GetString(29 + dayWeather);
                        g_nesne.Resim = reader.GetString(29 + dayWeather) + ".png";
                        g_nesne.EnDusukNem = reader.GetDecimal(19 + dayWeather);
                        g_nesne.EnYuksekNem = reader.GetDecimal(24 + dayWeather);
                        g_nesne.RuzgarHizi = reader.GetDecimal(34 + dayWeather);
                        g_nesne.RuzgarYon = reader.GetDecimal(39 + dayWeather);
                        gunlukNesne.Add(g_nesne);
                    }
                }
                else
                {
                    int indexSayac = 0;
                    reader = db.CommandReader($"SELECT * FROM DailyForecasts WHERE (InsertDate BETWEEN '{date}' AND '{date1}')");
                    while (reader.Read())
                    {

                        if (Convert.ToInt32(reader.GetInt32(2)) % 100 == 1)
                        {
                            int index = Nesneler.FindIndex(a => a.GunlukIstasyon == reader.GetInt32(2));

                            g_nesne = new GunlukDurum();

                            g_nesne.ilAdi = Nesneler[index].ilAd;
                            g_nesne.lat = Nesneler[index].lat;
                            g_nesne.lang = Nesneler[index].lang;
                            g_nesne.ilceAdi = Nesneler[index].ilceAd;
                            g_nesne.GunlukistNo = Nesneler[index].GunlukIstasyon;
                            g_nesne.EnDusukSicaklik = reader.GetDecimal(9 + dayWeather);
                            g_nesne.EnyuksekSicaklik = reader.GetDecimal(14 + dayWeather);
                            g_nesne.hadiseKodu = reader.GetString(29 + dayWeather);
                            g_nesne.Resim = reader.GetString(29 + dayWeather) + ".png";
                            g_nesne.EnDusukNem = reader.GetDecimal(19 + dayWeather);
                            g_nesne.EnYuksekNem = reader.GetDecimal(24 + dayWeather);
                            g_nesne.RuzgarHizi = reader.GetDecimal(34 + dayWeather);
                            g_nesne.RuzgarYon = reader.GetDecimal(39 + dayWeather);
                            if (indexSayac == 0 || gunlukNesne[indexSayac - 1].GunlukistNo != g_nesne.GunlukistNo)
                            {
                                gunlukNesne.Add(g_nesne);
                                indexSayac++;
                            }

                        }
                    }
                }
                return Json(gunlukNesne, JsonRequestBehavior.AllowGet);
            }
            else if (veriler[1] == "2")
            {
                List<SaatlikDurum> saatlikNesne_;
                int DataHours = ((int.Parse(veriler[2])) - 1);
                int saat = 0;
                saatlikNesne_ = SaatlikIstek(db, reader, Nesneler, veriler, saat);
                while (saatlikNesne_.Count < 70)
                {
                    reader.Close();
                    saat += 3;
                    saatlikNesne_ = SaatlikIstek(db, reader, Nesneler, veriler, saat);
                }
                return Json(saatlikNesne_, JsonRequestBehavior.AllowGet);
            }
            db.CloseConnection();
            return Json(Nesneler, JsonRequestBehavior.AllowGet);
        }
        public JsonResult IlceGetir(string il_adi)
        {
            List<string> ilceler = new List<string>();
            DBMysql db = new DBMysql();
            db.OpenConnection();
            MySqlDataReader reader = db.CommandReader($"select * from CityMasterTable WHERE ilAdi='{il_adi}'");

            while (reader.Read())
            {
                ilceler.Add(reader.GetString(1));
            }
            db.CloseConnection();
            return Json(ilceler, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SondurumGetir(string[] dataliste)
        {
            AnlikDurum anlikNesne = new AnlikDurum();
            DBMysql db = new DBMysql();
            db.OpenConnection();
            string dataReq = DateTime.Now.ToString($"{dataliste[2]} {dataliste[3]}");
            MySqlDataReader reader = db.CommandReader($"select * from LastStateActual WHERE ilAdi='{dataliste[0]}' AND ilceAdi='{dataliste[1]}' AND InsertDate='{dataReq}'");
            while (reader.Read())
            {
                anlikNesne.AktuelBasinc = reader.GetDecimal(4);  
                anlikNesne.DenizSicaklik = reader.GetDecimal(5);    
                anlikNesne.DenizIndirgenmisBasinc = reader.GetDecimal(6); 
                anlikNesne.Gorus = reader.GetDecimal(7); 
                anlikNesne.HadiseKodu = reader.GetString(8);
                anlikNesne.Kapalilik = reader.GetInt32(9);
                anlikNesne.KarYukseklik = reader.GetDecimal(10); 
                anlikNesne.Nem = reader.GetDecimal(11);
                anlikNesne.RasatMetar = reader.GetString(12);
                anlikNesne.RasatSinoptik = reader.GetString(13);
                anlikNesne.RasatTaf = reader.GetString(14);
                anlikNesne.RuzgarHizi = reader.GetDecimal(15);
                anlikNesne.RuzgarYon  = reader.GetDecimal(16);
                anlikNesne.Sicaklik = reader.GetDecimal(17);
                anlikNesne.VeriZamani = reader.GetDateTime(18);
                anlikNesne.Yagis00now = reader.GetDecimal(19);
                anlikNesne.Yagis10dk  = reader.GetDecimal(20);
                anlikNesne.Yagis12Saat= reader.GetDecimal(21);
                anlikNesne.Yagis1Saat = reader.GetDecimal(22);
                anlikNesne.Yagis24Saat= reader.GetDecimal(23);
                anlikNesne.Yagis6Saat = reader.GetDecimal(24);   
                anlikNesne.DenizVeriZamani = reader.GetDateTime(25);
            }
            return Json(anlikNesne, JsonRequestBehavior.AllowGet);
        }
    }
}