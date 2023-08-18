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

        public IEnumerable<Creature> GetCreatures()
        {
            return Terraria.Instance.Creatures;
        }

        public Creature AddCreature(Creature request)
        {
            Terraria.Instance.NewCreature(request);
            return request;
        }

        public Creature Spell(int skillID, int sourceID, int targetID)
        {
            var sourceCreature = Terraria.Instance.Creatures.FirstOrDefault(x => x.ID == sourceID);
            if(sourceCreature ==null)
                throw new Exception("Creature not found.");
            var targetCreature = Terraria.Instance.Creatures.FirstOrDefault(x => x.ID == targetID);
            if(targetCreature ==null)
                throw new Exception("Creature not found.");
            if(!sourceCreature.Skills.Contains(skillID))
                throw new Exception("Creature does not know this skill.");
            var effectSkill = SkillPool.Instance.Skills.FirstOrDefault(x => x.ID == skillID);
            if(effectSkill == null)
                throw new Exception("Skill not found.");

            return Spell(effectSkill, sourceCreature, targetCreature);

            Creature Spell(Skill skill, Creature source, Creature target)
            {
                switch (skill.EffectType)
                {
                    case EffectType.Damage:
                        return Attack(skill, source, target);
                    case EffectType.Heal:
                        return Heal(skill, source, target);
                    default:
                        return target;
                }
            }

            Creature Attack(Skill skill, Creature source, Creature target)
            {
                var baseDamage = RollEffect(skill);
                if (baseDamage >= target.ArmorClass.Value)
                {
                    var extraDamage = (source.Dexterity.Value > target.Dexterity.Value) ? Dice.Roll(sourceCreature.AttackBonus.Value) : 0;
                    var armor = target.ArmorClass.Value;
                    var hurt = baseDamage + extraDamage - armor;
                    Console.WriteLine($"{source.Name} attacks {target.Name} {target.Health.Value}/{target.Health.MaxValue} for {hurt}.");
                    if (hurt > 0)
                        target.Health.Sub(hurt);
                }
                Terraria.Instance.Update(target);
                if (target.Health.Value <= 0)
                {
                    Console.WriteLine($"{target.Name} has died.");
                    _creatureService.AddExperience(source.ID, target.Level.Value);
                }
                return target;
            }

            Creature Heal(Skill skill, Creature source, Creature target)
            {
                var heal = RollEffect(skill);
                Console.WriteLine($"{source.Name} heals {target.Name} {target.Health.Value}/{target.Health.MaxValue} for {heal}.");
                target.Health.Add(heal);
                return target;
            }

            int RollEffect(Skill skill)
            {
                return Dice.Roll(skill.MaxValue, skill.Value);
            }
        }
    }
}
