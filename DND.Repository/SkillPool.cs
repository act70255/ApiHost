using DND.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DND.Repository
{
    public sealed class SkillPool : BaseRepository
    {
        private static readonly Lazy<SkillPool> lazy = new Lazy<SkillPool>(() => new SkillPool());
        public static SkillPool Instance { get { return lazy.Value; } }

        public IEnumerable<Skill> Skills => GetList<Skill>();

        SkillPool()
        {
            var skillList = new List<Skill>();
            InsertRange<Skill>(GeneratePhysicalSkills());
            InsertRange<Skill>(GenerateFireSkills());
            InsertRange<Skill>(GenerateWaterSkills());
            InsertRange<Skill>(GenerateHealSkills());
            //SaveSkills(skillList);
            //Skills.AddRange(GenerateBuffSkills());
            //Skills.AddRange(GenerateDebuffSkills());
        }
        List<Skill> GeneratePhysicalSkills()
        {
            return new List<Skill>()
            {
                new Skill(0, "Melee", ActionType.Attack, ElementType.Physical, EffectType.Damage, 0, 0, 2, false, 10, true, 2, 7),
                new Skill(1,"Shoot", ActionType.Attack, ElementType.Physical, EffectType.Damage, 0, 0, 18, false, 10, true, 2, 7),
            };
        }
        List<Skill> GenerateFireSkills()
        {
            return new List<Skill>()
            {
                new Skill(10,"FireArrow", ActionType.Attack, ElementType.Fire, EffectType.Damage, 3, 0, 18, false, 10, true, 3, 11),
                new Skill(11,"Fireball", ActionType.Attack, ElementType.Fire, EffectType.Damage, 5, 0, 18, true, 3, true, 4, 8),
                new Skill(12,"FireStorm", ActionType.Attack, ElementType.Fire, EffectType.Damage, 10, 0, 18, true, 5, true, 5, 20),
            };
        }
        List<Skill> GenerateWaterSkills()
        {
            return new List<Skill>
            {
                new Skill (20,"IceArrow", ActionType.Attack, ElementType.Water, EffectType.Damage, 3, 0, 18, false, 10, true, 3, 11),
                new Skill (21,"IceBall", ActionType.Attack, ElementType.Water, EffectType.Damage, 5, 0, 18, true, 3, true, 4, 8),
                new Skill (22,"Blizzard", ActionType.Attack, ElementType.Water, EffectType.Damage, 10, 0, 18, true, 5, true, 5, 20),
            };
        }
        List<Skill> GenerateHealSkills()
        {
            return new List<Skill>
            {
                new Skill (30,"Heal", ActionType.Heal, ElementType.Light, EffectType.Heal, 3, 0, 18, false, 10, true, 3, 7),
                new Skill (31,"Cure", ActionType.Heal, ElementType.Light, EffectType.Heal, 3, 0, 4, false, 10, true, 7, 11),
                new Skill (32,"GroupHeal",ActionType.Heal, ElementType.Light, EffectType.Heal, 5, 0, 18, true, 4, true, 4, 8),
                new Skill (33,"GreatHeal", ActionType.Heal, ElementType.Light, EffectType.Heal, 6, 0, 18, false, 10, true, 7, 17),
            };
        }
    }
}
