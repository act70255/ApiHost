using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace ApiHost.Forms.Controller
{
    public class FormController : ApiController
    {
        [HttpGet]
        public IHttpActionResult MachineName()
        {
            return Json(new { HostName = Environment.CurrentDirectory });
        }
        [HttpGet]
        public IHttpActionResult HostTime()
        {
            return Json(new { HostTime = DateTime.Now });
        }
    }
}
