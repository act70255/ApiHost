using ApiHost.DND.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiHost.DND
{
    public sealed class Dice
    {
        private static readonly Lazy<Dice> lazy = new Lazy<Dice>(() => new Dice());
        public static Dice Instance { get { return lazy.Value; } }
        private static readonly Random random = new Random();
        private static readonly object syncLock = new object();
        public static int Roll(int sides = 20,int start = 0)
        {
            lock (syncLock)
            {
                return random.Next(start + 1, sides + 1);
            }
        }
    }
    public sealed class SkillPool
    {
        private static readonly Lazy<SkillPool> lazy = new Lazy<SkillPool>(() => new SkillPool());
        public static SkillPool Instance { get { return lazy.Value; } }
        List<Skill> _skills { get; set; } = new List<Skill>();

        public List<Skill> Skills => _skills;

        SkillPool()
        {
            _skills.AddRange(GeneratePhysicalSkills());
            _skills.AddRange(GenerateFireSkills());
            _skills.AddRange(GenerateWaterSkills());
            _skills.AddRange(GenerateHealSkills());
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

    public sealed class Terraria
    {
        private static readonly Lazy<Terraria> lazy = new Lazy<Terraria>(() => new Terraria());
        public static Terraria Instance { get { return lazy.Value; } }

        List<Creature> creatures { get; set; } = new List<Creature>();

        public List<Creature> Creatures => creatures;

        public Creature NewCreature(string name, int health = 10, int mana = 10, int stamina = 10, int experience = 20, int level = 10, int armorClass = 10, int attackBonus = 10, int damage = 10, int strength = 10, int dexterity = 10, int intelligence = 10, int charisma = 10)
        {
            var newID = creatures.Max(m => m.ID) + 1;
            var creature = new Creature(newID, name, health, mana, stamina, experience, level, armorClass, attackBonus, damage, strength, dexterity, intelligence, charisma);
            creatures.Add(creature);
            return creature;
        }
        public Creature NewCreature(Creature creature)
        {
            creature.ID = creatures.Any() ? creatures.Max(m => m.ID) + 1 : 0;
            creatures.Add(creature);
            return creature;
        }
        public void Clear()
        {
            creatures.Clear();
        }
    }
}
