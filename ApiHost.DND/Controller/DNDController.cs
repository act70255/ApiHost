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
        ITerrariaService _terriarService;
        IMapper _mapper;

        public DNDController(ILogger logger, ITerrariaService terriarService, IMapper mapper)
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
            return Json(_mapper.Map<IEnumerable<CreatureRequest>>(_terriarService.GetCreatures()));
        }

        [HttpPost]
        public IHttpActionResult NewCreatures(CreatureRequest request)
        {
            if (request != null)
                _terriarService.AddCreature(request);
            else
                _terriarService.AddCreature(new CreatureRequest("NewCharcater"));

            return Ok();
        }

        [HttpPost]
        public IHttpActionResult Action(ActionRequest request)
        {
            var result = _terriarService.Action(request.skill, request.source, request.target);
            var response = _mapper.Map<CreatureRequest>(result);
            return Json(response);
        }

        [HttpPost]
        public IHttpActionResult GetSkills(SkillsRequest request)
        {
            if (request != null && request.ids.Any())
                return Json(SkillPool.Instance.Skills.Where(x => request.ids.Contains(x.ID)));
            else
                return Json(SkillPool.Instance.Skills);
        }
    }
}
