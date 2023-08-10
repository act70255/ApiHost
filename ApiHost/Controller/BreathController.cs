using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace ApiHost.Controller
{
    public class BreathController : ApiController
    {
        [HttpGet]
        public IHttpActionResult Get()
        {
            return Json(new { SystemTime = DateTime.Now });
        }
        [HttpPost]
        public IHttpActionResult Post(string req)
        {
            return Json(new { HostName = Environment.MachineName});
        }
    }
}
