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
            return creature;
        }

        public Creature Spell(int skillID, int sourceID, int targetID)
        {
            var sourceCreature = Terraria.Instance.Creatures.FirstOrDefault(x => x.ID == sourceID);
            var targetCreature = Terraria.Instance.Creatures.FirstOrDefault(x => x.ID == targetID);
            var effectSkill = SkillPool.Instance.Skills.FirstOrDefault(x => x.ID == skillID);
            if (sourceCreature == null || targetCreature == null || effectSkill == null)
            {
                throw new Exception("Creature or Skill not found.");
            }
            return Spell(effectSkill, sourceCreature, targetCreature);

            Creature Spell(Skill skill, Creature source, Creature target)
            {
                switch (skill.EffectType)
                {
                    //case EffectType.Damage:
                    //    return Attack(source, target);
                    case EffectType.Heal:
                        return Heal(skill, source, target);
                    default:
                        return target;
                }
            }

            //Creature Attack(Creature source, Creature target)
            //{
            //    var attack = RollEffect(source.AttackBonus.Value, 20 - source.Strength.Value);
            //    if (source.Damage.Value >= target.ArmorClass.Value)
            //    {
            //        var damage = RollEffect(source.Damage.Value, source.Dexterity.Value - target.Dexterity.Value) + RollEffect(source.AttackBonus.Value);
            //        var armor = RollEffect(target.ArmorClass.Value);
            //        var hurt = damage - armor;
            //        Console.WriteLine($"{source.Name} attacks {target.Name} {target.Health.Value}/{target.Health.MaxValue} for {hurt}.");
            //        if (hurt > 0)
            //            target.Health.Sub(hurt);
            //    }
            //    return target;
            //}

            Creature Heal(Skill skill, Creature source, Creature target)
            {
                var heal = RollEffect(skill);
                Console.WriteLine($"{source.Name} heals {target.Name} {target.Health.Value}/{target.Health.MaxValue} for {heal}.");
                target.Health.Add(heal);
                return target;
            }
        }

        public Creature AddSkills(int creatureID, int[] skillIDs)
        {
            var creature = Terraria.Instance.Creatures.FirstOrDefault(x => x.ID == creatureID);
            if (creature == null)
                throw new Exception("Creature not found.");
            var skills = SkillPool.Instance.Skills.Where(s => skillIDs.Contains(s.ID));
            if (skills == null || !skills.Any())
                throw new Exception("Skill not found.");

            var newSkills = skills.Where(a => !creature.Skills.Contains(a.ID));

            creature.Skills.AddRange(newSkills.Select(s => s.ID));
            return creature;
        }


        public int RollEffect(Skill skill)
        {
            return Dice.Roll(skill.MaxValue, skill.Value);
        }
    }
}
