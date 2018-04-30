using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BitirmeProjesi.MyDatabase
{
    public class DBMysql
    {
        MySqlConnection connection;
        //Baglanti adında bir bağlantı oluşturdum
        MySqlDataReader rdr = null;
        public bool OpenConnection()
        {
            try
            {
                connection = new MySqlConnection("Server=programteknik.com;Database=program1_WeatherDB;Uid=program1_WeatherUser;Pwd='sabanselim';");
                connection.Open();
                return true;
                //Veritabanına bağlanırsa baglanti_kontrol fonksiyonu "true" değeri gönderecek
            }
            catch (Exception)
            {
                return false;
                //Veritabanına bağlanamazsa "false" değeri dönecek
            }
        }

        public bool Command(string command)
        {
            try
            {
                MySqlCommand commandMysql = connection.CreateCommand();
                commandMysql.CommandText = command;
                commandMysql.ExecuteNonQuery();
                /*
                MySqlDataReader mSqlReader_Customers;
                mSqlReader_Customers = commandMysql.ExecuteReader();
                DataTable dtCustomers = new DataTable();
                dtCustomers.Load(mSqlReader_Customers);
                foreach (DataRow row in dtCustomers.Rows)
                {
                    Console.WriteLine(row[1].ToString());
                }
                */
                return true;
                //Veritabanına bağlanırsa baglanti_kontrol fonksiyonu "true" değeri gönderecek
            }

            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return false;
                //Veritabanına bağlanamazsa "false" değeri dönecek
            }
        }
        public MySqlDataReader CommandReader(string command)
        {
            MySqlCommand commandMysql = connection.CreateCommand();
            commandMysql.CommandText = command;
            return commandMysql.ExecuteReader();

        }
        public bool CloseConnection()
        {
            connection.Close();
            return true;
        }
    }
}
