using ApiHost.DND.Controller;
using ApiHost.DND.Model;
using ApiHost.DND.Service.Interface;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiHost.DND.Service
{
    public class TerrariaService : ITerrariaService
    {
        IMapper _mapper;
        ICreatureService _creatureService;

        public TerrariaService(ICreatureService creatureService, IMapper mapper)
        {
            _creatureService = creatureService;
            _mapper = mapper;
        }

        public List<Creature> GetCreatures()
        {
            return Terraria.Instance.Creatures;
        }

        public Creature AddCreature(CreatureRequest request)
        {
            var creature = _mapper.Map<Creature>(request);
            Terraria.Instance.NewCreature(creature);
            return creature;
        }
    }
}
