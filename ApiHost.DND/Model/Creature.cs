using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ApiHost.DND.Model
{
    public class Creature : BaseElement
    {
        public Creature() { }
        public Creature(int id, string name, int health = 10, int mana = 10, int stamina = 10, int experience = 20, int level = 10, int armorClass = 10, int attackBonus = 10, int damage = 10, int strength = 10, int dexterity = 10, int intelligence = 10, int charisma = 10)
        {
            ID = id;
            Name = name;
            Health = new StatusVolume("Health", health, health);
            Mana = new StatusVolume("Mana", mana, mana);
            Stamina = new StatusVolume("Stamina", stamina, stamina);
            Experience = new StatusVolume("Experience", 0, experience);
            Level = new StatusVolume("Level", 1, level);
            ArmorClass = new StatusValue("ArmorClass", armorClass);
            AttackBonus = new StatusValue("AttackBonus", attackBonus);
            Damage = new StatusValue("Damage", damage);
            Strength = new StatusValue("Strength", strength);
            Dexterity = new StatusValue("Dexterity", dexterity);
            Intelligence = new StatusValue("Intelligence", intelligence);
            Charisma = new StatusValue("Charisma", charisma);
        }
        public override string Name { get; set; }
        public StatusVolume Health { get; set; }
        public StatusVolume Mana { get; set; }
        public StatusVolume Stamina { get; set; }
        public StatusVolume Experience { get; set; }
        public StatusVolume Level { get; set; }

        public StatusValue ArmorClass { get; set; }
        public StatusValue AttackBonus { get; set; }
        public StatusValue Damage { get; set; }

        public StatusValue Strength { get; set; }
        public StatusValue Dexterity { get; set; }
        public StatusValue Intelligence { get; set; }
        public StatusValue Charisma { get; set; }
        public List<int> Skills { get; set; } = new List<int>();
    }
}
