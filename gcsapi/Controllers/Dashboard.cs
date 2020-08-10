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
    public class Dashboard
    {
        string json;
        DataSet ds = new DataSet();
        public static string ConnectionString = "Server=alta.telkom.space;Initial Catalog=GCS;User Id=gcs;Password=telk0mS4t!";
        public static SqlConnection sqlCon = new SqlConnection(Settings.ConnectionString);
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        [HttpPost]
        public string logbook([FromBody] object value)
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
                    /*var result = new                                                      //Menambahkan object didepan
                    {
                        user = listEmployees
                    };*/
                    json = JsonConvert.SerializeObject(listEmployees, Formatting.Indented);
                }
                else
                {
                    //json = "{\r\n \"Table\":[\r\n {\r\n \"status\":\"gagal\"\r\n}\r\n ]\r\n}";
                    dataUser.status = "gagal";
                    listEmployees.Add(dataUser);
                    json = JsonConvert.SerializeObject(listEmployees, Formatting.Indented);
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
        public string LogNow()
        {
            string now = DateTime.Now.ToString("yyyy/MM/dd");
            string query = $"select id_logbook, unit, tipe_logbook, judul_logbook from tabel_logbook where tanggal = '{now}'";
            DataSet ds = Settings.LoadDataSet(query);
            string json = JsonConvert.SerializeObject(ds, Formatting.Indented);

            return json;
        }

        [HttpGet]
        public string LogOnProgress()
        {
            string now = DateTime.Now.ToString("yyyy/MM/dd");
            string query = $"select top 3 id_logbook, judul_logbook from tabel_logbook where status = 'On Progress' order by id_logbook desc ";
            DataSet ds = Settings.LoadDataSet(query);
            string json = JsonConvert.SerializeObject(ds, Formatting.Indented);

            return json;
        }

        [HttpGet]
        public string shiftme2()
        {
            var mulai = DateTime.Now.AddDays(-1).ToString("yyyy/MM/dd");
            var akhir = DateTime.Now.AddDays(1).ToString("yyyy/MM/dd");

            string query = $@"SELECT m.tanggal_shift, jadwal, STUFF((SELECT ',  ' + p.petugas 
                      FROM shiftme s left join shiftme_petugas p on s.id_petugas = p.id_petugas
					  WHERE m.jadwal = s.jadwal and m.tanggal_shift = s.tanggal_shift
                      FOR XML PATH('')), 1, 1, '') [petugas] FROm shiftme m WHERE tanggal_shift >= '{mulai}' and tanggal_shift <= '{akhir}'
					  group by tanggal_shift, jadwal
                      order by tanggal_shift asc, jadwal asc ";

            DataSet ds = Settings.LoadDataSet(query);
            string json = JsonConvert.SerializeObject(ds, Formatting.Indented);

            return json;
        }

        [HttpGet]
        public string shiftme()
        {
            List<DataShift> listEmployees = new List<DataShift>();
            
            var now = DateTime.Now.ToString("yyyy/MM/dd");
            var akhir = DateTime.Now.AddDays(1).ToString("yyyy/MM/dd");

            string query = $@"select tanggal_shift, jadwal from shiftme m join shiftme_petugas p on p.id_petugas=m.id_petugas 
                            where m.tanggal_shift='2020/08/05' group by tanggal_shift, jadwal order by jadwal";
            
            
            DataSet a = Settings.LoadDataSet(query);
            SqlDataAdapter da;
            
            if (a.Tables[0].Rows.Count > 0)
            {
                for(int j = 0; j < a.Tables[0].Rows.Count; j++)
                {
                    ds.Clear();
                    DataShift dataUser = new DataShift();

                    dataUser.jadwal = a.Tables[0].Rows[j]["jadwal"].ToString();
                    dataUser.tanggal = a.Tables[0].Rows[j]["tanggal_shift"].ToString();
                    DateTime tgl = Convert.ToDateTime(a.Tables[0].Rows[j]["tanggal_shift"]);
                    string mydate = tgl.ToString("yyyy/MM/dd");
                    string jadwal = a.Tables[0].Rows[j]["jadwal"].ToString();

                    string query2 = $@"select p.petugas, e.foto from shiftme s left join shiftme_petugas p on s.id_petugas = p.id_petugas
                            full join Profile e on e.id_profile=p.id_profile
					        WHERE s.jadwal = '{jadwal}' and s.tanggal_shift = '{mydate}'";

                    SqlCommand cmd1 = new SqlCommand(query2, sqlCon);
                    da = new SqlDataAdapter(cmd1);
                    da.Fill(ds);
                    sqlCon.Open();
                    cmd1.ExecuteNonQuery();
                    sqlCon.Close();
                    List<Petugas> listEmployees2 = new List<Petugas>();

                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        Petugas dataUser2 = new Petugas();
                        dataUser2.nama = ds.Tables[0].Rows[i]["petugas"].ToString();
                        dataUser2.foto = ds.Tables[0].Rows[i]["foto"].ToString().Replace("~", "gcs.telkom.space");
                        listEmployees2.Add(dataUser2);

                        dataUser.petugas = listEmployees2;
                    }

                    
                    listEmployees.Add(dataUser);
                }
                
                /*var result = new                                                      //Menambahkan object didepan
                {
                    user = listEmployees
                };*/
                json = JsonConvert.SerializeObject(listEmployees, Formatting.Indented);
            }
            return json;
        }
    }
}
