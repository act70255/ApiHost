﻿using ApiHost.Host.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;

namespace ApiHost.Spider.Controller
{
    public class postController : ApiController
    {
        [HostMessaging("POSTnull",null,"")]
        [HttpPost]
        public IHttpActionResult log([FromBody] dynamic queyr)
        {
            return Ok();
        }
        [HttpPost]
        public IHttpActionResult Data([FromBody] dynamic queyr)
        {
            Console.WriteLine($"[{DateTime.Now} Data] {Newtonsoft.Json.JsonConvert.SerializeObject(queyr)}");
            return Ok();
        }
    }
}
