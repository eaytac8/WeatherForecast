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
            
            List<string> iller = new List<string>();
            List<SelectListItem> Liste_il = new List<SelectListItem>();
            db.OpenConnection();
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


            return View();
        }
        public JsonResult VerileriGetir()
        {
            DBMysql db = new DBMysql();

            List<model> nesneListem = new List<model>();
            model md;
            db.OpenConnection();
            List<SelectListItem> Liste_il = new List<SelectListItem>();
            MySqlDataReader reader = db.CommandReader("select * from CityMasterTable");

            while (reader.Read())
            {
                if (reader.GetInt32(2) > 0)
                {
                    md = new model();
                    md.lat = reader.GetDouble(4);
                    md.lang = reader.GetDouble(5);
                    md.ilAd = reader.GetString(0);
                    md.GunlukIstasyon = reader.GetInt32(6);
                    nesneListem.Add(md);

                    //9 en düşük 
                    //14 en yüksek
                    //29 hadise
                }
            }
            reader.Close();

            string date = DateTime.Now.ToString("yyyy-MM-dd 03:00:00");
            
            reader = db.CommandReader($"SELECT * FROM DailyForecasts WHERE InsertDate='{date}'");

            while (reader.Read())
            {

                if (Convert.ToInt32(reader.GetInt32(2)) % 100 == 1)
                {
                    int index = nesneListem.FindIndex(a => a.GunlukIstasyon == reader.GetInt32(2));

                    nesneListem[index].EnDusukSicaklik = reader.GetDecimal(9);
                    nesneListem[index].EnyuksekSicaklik = reader.GetDecimal(14);
                    nesneListem[index].hadiseKodu = reader.GetString(29);
                }

            }
            
            return Json(nesneListem, JsonRequestBehavior.AllowGet);
        }
    }
}