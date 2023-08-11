using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace ApiHost.Host.Controller
{
    public class HostController : ApiController
    {
        [HttpGet]
        public IHttpActionResult MachineName()
        {
            return Json(new { HostName = Environment.MachineName });
        }
        [HttpGet]
        public IHttpActionResult HostTime()
        {
            return Json(new { HostTime = DateTime.Now });
        }
        [HttpPost]
        public IHttpActionResult Data([FromBody] dynamic queyr)
        {
            Console.WriteLine($"[{DateTime.Now} Data] {Newtonsoft.Json.JsonConvert.SerializeObject(queyr)}");
            return Ok();
        }
    }
}
