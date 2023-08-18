using DND.Model;
using DND.Domain.Service.Interface;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DND.Repository;

namespace DND.Domain.Service
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

        public Creature AddCreature(Creature request)
        {
            Terraria.Instance.NewCreature(request);
            return request;
        }
    }
}
