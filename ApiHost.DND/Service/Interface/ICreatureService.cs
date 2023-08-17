using ApiHost.DND.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiHost.DND.Service.Interface
{
    public interface ICreatureService
    {
        Creature Action(int skill, int name, int target);
    }
}
