using ApiHost.DND.Model;
using ApiHost.DND.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiHost.DND.Service
{
    public class TerriarService: ITerriarService
    {
        static List<Creature> Creatures = new List<Creature>();
        public TerriarService()
        {
            Creatures.Add(new Creature("PC_0"));
            Creatures.Add(new Creature("PC_1"));
            Creatures.Add(new Creature("PC_2"));
            Creatures.Add(new Creature("PC_3"));
        }

        public List<Creature> GetCreatures()
        {
            return Creatures;
        }

        public void AddCreature(string name)
        {
        }

        public Creature Action(ActionType action, string name, string target)
        {
            var creature = Creatures.FirstOrDefault(x => x.Name == name);
            var targetCreature = Creatures.FirstOrDefault(x => x.Name == target);
            if (creature != null && targetCreature != null)
            {
                return creature.Action(action, targetCreature);
            }
            return targetCreature;
        }

        public Creature GainExperience(string name, int experience)
        {
            var creature = Creatures.FirstOrDefault(x => x.Name == name);
            return creature;
        }
    }
}
