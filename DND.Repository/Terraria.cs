using DND.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DND.Repository
{
    public sealed class Terraria : BaseRepository
    {
        private static readonly Lazy<Terraria> lazy = new Lazy<Terraria>(() => new Terraria());
        public static Terraria Instance { get { return lazy.Value; } }

        public IEnumerable<Creature> Creatures => GetList<Creature>();

        public Creature NewCreature(string name, int health = 10, int mana = 10, int stamina = 10, int experience = 20, int level = 10, int armorClass = 10, int attackBonus = 10, int damage = 10, int strength = 10, int dexterity = 10, int intelligence = 10, int charisma = 10)
        {
            var newID = Creatures.Max(m => m.ID) + 1;
            var creature = new Creature(newID, name, health, mana, stamina, experience, level, armorClass, attackBonus, damage, strength, dexterity, intelligence, charisma);
            Insert(creature);
            return creature;
        }
        public Creature NewCreature(Creature creature)
        {
            creature.ID = Creatures.Any() ? Creatures.Max(m => m.ID) + 1 : 0;
            Insert(creature);
            return creature;
        }
        public void Clear()
        {
            Save(new List<Creature>());
        }
    }
}
