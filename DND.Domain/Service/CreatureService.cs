using DND.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using System.Xml.Linq;
using DND.Domain.Service.Interface;
using DND.Repository;

namespace DND.Domain.Service
{
    public class CreatureService : ICreatureService
    {
        public Creature AddExperience(int sourceID, int value)
        {
            var creature = Terraria.Instance.Creatures.FirstOrDefault(x => x.ID == sourceID);
            if(creature == null)
                throw new Exception("Creature not found");

            while (creature.Experience.CheckAdd(value))
            {
                value = creature.Experience.Add(value);
                creature.Level.Add(1);
                creature.Experience.MaxValue = Convert.ToInt32(Math.Pow(creature.Level.Value, 0.5)) * 20;
                creature.Experience.Value = 0;
            }
            Terraria.Instance.Update(creature);
            return creature;
        }

        public Creature AddSkills(int creatureID, int[] skillIDs)
        {
            var creature = Terraria.Instance.Creatures.FirstOrDefault(x => x.ID == creatureID);
            if (creature == null)
                throw new Exception("Creature not found.");
            var skills = SkillPool.Instance.Skills.Select(s => s.ID).Where(f => skillIDs.Contains(f));
            if (skills == null || !skills.Any())
                throw new Exception("Skill not found.");

            var newSkills = skills.Where(a => !creature.Skills.Contains(a));

            creature.Skills = creature.Skills.Concat(newSkills).Distinct().OrderBy(o => o).ToArray();
            Terraria.Instance.Update(creature);
            return creature;
        }
    }
}
