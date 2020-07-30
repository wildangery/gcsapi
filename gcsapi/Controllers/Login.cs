using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Security.Cryptography;

namespace gcsapi.Controllers
{

    [ApiController]
    [Route("[controller]/[action]")]
    public class Login : ControllerBase
    {
        string json;
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        [HttpPost]
        public string PostTest([FromBody] object value)
        {
            try
            {
                Userdata oLogin = JsonConvert.DeserializeObject<Userdata>(value.ToString());
                
                string insertdata = $"SELECT id_profile, user_name FROM Profile WHERE user_name='" + oLogin.user + "' AND password ='" + Settings.ToMD5Hash(Settings.ToMD5Hash(oLogin.password)) + " '";
                DataSet a = Settings.LoadDataSet(insertdata);

                if(a.Tables[0].Rows.Count > 0)
                {
                    json = JsonConvert.SerializeObject(a, Formatting.None);
                }
                else
                {
                    //json = "{\r\n \"Table\":[\r\n {\r\n \"status\":\"gagal\"\r\n}\r\n ]\r\n}";
                    json = JsonConvert.SerializeObject("{\"Table\":[{\"status\":\"gagal\"}]}", Formatting.Indented);
                }
                return json;
                //return value;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }

        [HttpPost]
        public string UserLogin([FromBody] object value)
        {
            List<DataUser> listEmployees = new List<DataUser>();
            try
            {
                Userdata oLogin = JsonConvert.DeserializeObject<Userdata>(value.ToString());
                DataUser dataUser = new DataUser();
                string insertdata = $"SELECT id_profile, user_name FROM Profile WHERE user_name='" + oLogin.user + "' AND password ='" + Settings.ToMD5Hash(Settings.ToMD5Hash(oLogin.password)) + " '";
                DataSet a = Settings.LoadDataSet(insertdata);

                if (a.Tables[0].Rows.Count > 0)
                {
                    dataUser.idnama = a.Tables[0].Rows[0]["id_profile"].ToString();
                    dataUser.nama = a.Tables[0].Rows[0]["user_name"].ToString();
                    dataUser.status = "berhasil";
                    listEmployees.Add(dataUser);
                    json = JsonConvert.SerializeObject(listEmployees, Formatting.None);
                }
                else
                {
                    //json = "{\r\n \"Table\":[\r\n {\r\n \"status\":\"gagal\"\r\n}\r\n ]\r\n}";
                    dataUser.status = "gagal";
                    listEmployees.Add(dataUser);
                    json = JsonConvert.SerializeObject(listEmployees, Formatting.None);
                }
                return json;
                //return value;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }


        [HttpGet]
        public string GetData()
        {
            DataSet ds = Wilayah.GetWilayah();
            string Json = JsonConvert.SerializeObject(ds, Formatting.Indented);

            return Json;
        }

    }
}
