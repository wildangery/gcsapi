using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.AspNetCore.JsonPatch;

namespace gcsapi.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class Asset : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        [HttpGet]
        public string GetData()
        {
            DataSet ds = Wilayah.GetWilayah();
            string Json = JsonConvert.SerializeObject(ds, Formatting.Indented);

            return Json;
        }
        public string PostTest([FromBody] object value)
        {
            try
            {
                Ruangan oWilayah = JsonConvert.DeserializeObject<Ruangan>(value.ToString());

                string insertdata = $"INSERT INTO as_wilayah (nama_wilayah, tanggal) values ('{oWilayah.nama_wilayah}','{oWilayah.tanggal}');";
                string a = Settings.ExsecuteSql(insertdata);
                return a;
                //return value;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        [HttpGet]
        public string GetRack()
        {
            string query = "SELECT b.tanggal, w.id_ruangan, b.id_rak, w.nama_ruangan, b.nama_rak, b.qr_rak FROM as_rak b join as_ruangan w on b.id_ruangan=w.id_ruangan ";
            DataSet ds = Settings.LoadDataSet(query);
            string json = JsonConvert.SerializeObject(ds, Formatting.Indented);

            return json;
        }

    }
}
