using DND.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DND.Domain.Service.Interface
{
    public interface ITerrariaService
    {
        List<Creature> GetCreatures();
        Creature AddCreature(Creature request);
    }
}
