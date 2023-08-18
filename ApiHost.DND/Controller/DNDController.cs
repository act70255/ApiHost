using AutoMapper;
using AutoMapper.Configuration;
using DND.Domain.Service.Interface;
using DND.Model;
using DND.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace ApiHost.DNDHost.Controller
{
    public class DNDController : ApiController
    {
        ILogger _logger;
        ITerrariaService _terrariaService;
        ICreatureService _creatureService;
        IMapper _mapper;

        public DNDController(ILogger logger, ITerrariaService terrariaService, ICreatureService creatureService, IMapper mapper)
        {
            _logger = logger;
            _terrariaService = terrariaService;
            _creatureService = creatureService;
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
            try
            {

                //return Ok();
                return Json(_mapper.Map<IEnumerable<CreatureRequest>>(_terrariaService.GetCreatures()));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public IHttpActionResult NewCreatures(CreatureRequest request)
        {
            try
            {
                object result = null;
                if (request != null)
                    result = _terrariaService.AddCreature(_mapper.Map<Creature>(request));
                else
                    result = _terrariaService.AddCreature(_mapper.Map<Creature>(new CreatureRequest()));

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public IHttpActionResult Action(ActionRequest request)
        {
            try
            {
                var result = _creatureService.Spell(request.Skill, request.Source, request.Target);
                var response = _mapper.Map<CreatureRequest>(result);
                return Json(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public IHttpActionResult GetSkills(SkillsRequest request)
        {
            try
            {
                if (request != null && request.IDs.Any())
                    return Json(SkillPool.Instance.Skills.Where(x => request.IDs.Contains(x.ID)));
                else
                    return Json(SkillPool.Instance.Skills);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public IHttpActionResult AddSKills(SkillsRequest request)
        {
            try
            {
                var result = _creatureService.AddSkills(request.ID, request.IDs.ToArray());
                return Json(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
