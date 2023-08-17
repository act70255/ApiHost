using ApiHost.DND.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiHost.DND.Service.Interface
{
    public interface ITerriarService
    {
        List<Creature> GetCreatures();
        Creature GainExperience(string name, int experience);
        Creature Action(ActionType action, string name, string target);
    }
}
