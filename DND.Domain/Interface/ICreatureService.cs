using DND.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DND.Domain.Service.Interface
{
    public interface ICreatureService
    {
        Creature Spell(int skill, int name, int target);
        Creature AddSkills(int creatureID, int[] skillIDs);
        Creature AddExperience(int sourceID, int value);
    }
}
