using ApiHost.Host.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace ApiHost.CLI.Controller
{
    [Result]
    public class ConsoleController :ApiController
    {
        [HttpGet]
        public IHttpActionResult GetSystemStatus(string message)
        {
            Console.WriteLine(message);
            return Ok();
        }
        [HttpPost]
        public IHttpActionResult PostSystemStatus(Model.MonitorModel data)
        {
            Console.WriteLine($"[{DateTime.Now}] {Newtonsoft.Json.JsonConvert.SerializeObject(data)}");
            return Ok();
        }
    }
}
