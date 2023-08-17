using ApiHost.DND.Model;
using ApiHost.DND.Service;
using ApiHost.DND.Service.Interface;
using AutoMapper;
using AutoMapper.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace ApiHost.DND.Controller
{
    public class DNDController : ApiController
    {
        ILogger _logger;
        ITerriarService _terriarService;
        IMapper _mapper;

        public DNDController(ILogger logger, ITerriarService terriarService, IMapper mapper)
        {
            _logger = logger;
            _terriarService = terriarService;
            _mapper = mapper;
        }
        [HttpPost]
        public IHttpActionResult Data([FromBody] dynamic queyr)
        {
            _logger.Write("", queyr);
            return Ok();
        }

        [HttpPost]
        public IHttpActionResult GetCreatures()
        {
            //return Ok();
            return Json(_terriarService.GetCreatures());
        }

        [HttpPost]
        public IHttpActionResult Action(ActionRequest queyr)
        {
            var result = _terriarService.Action(queyr.action, queyr.source, queyr.target);
            var response = _mapper.Map<CreatureRequest>(result);
            return Json(response);
        }
    }
}
