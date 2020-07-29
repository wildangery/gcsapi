using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;


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
