using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StatsForNerf.ConsoleApp;

namespace CSStatsForNerf.Controllers
{
    public class HomeController : Controller
    {
        [HttpPost("/", Name = "")]
        public IActionResult Index()
        {
            using (var reader = new StreamReader(Request.Body))
            {
                var body = reader.ReadToEnd();

                Task.Factory.StartNew(() =>
                {
                    var obj = JsonConvert.DeserializeObject<Model>(body);
                    TwitchConnector.Execute(obj);
                });

            }
            return Ok();
        }
    }
}