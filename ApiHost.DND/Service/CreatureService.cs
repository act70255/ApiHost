﻿using ApiHost.DND.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using System.Xml.Linq;

namespace ApiHost.DND.Service
{
    public class CreatureService
    {
        public void AddExperience(Creature source, int value)
        {
            while (source.Experience.CheckAdd(value))
            {
                value = source.Experience.Add(value);
                source.Level.Add(1);
                source.Experience.MaxValue = Convert.ToInt32(Math.Pow(source.Level.Value, 0.5)) * 20;
                source.Experience.Value = 0;
            }
        }
        public Creature Action(ActionType actionType, Creature source, Creature target)
        {
            switch (actionType)
            {
                case ActionType.Attack:
                    return Attack(source,target);
                case ActionType.Heal:
                    return Heal(source,target);
                default:
                    return target;
            }
        }

        public Creature Spell(int skillID Creature source, Creature target)
        {
            return target;
        }

        public Creature Attack(Creature source, Creature target)
        {
            var attack = RollEffect(source.AttackBonus.Value, 20 - source.Strength.Value);
            if (source.Damage.Value >= target.ArmorClass.Value)
            {
                var damage = RollEffect(source.Damage.Value, source.Dexterity.Value - target.Dexterity.Value) + RollEffect(source.AttackBonus.Value);
                var armor = RollEffect(target.ArmorClass.Value);
                var hurt = damage - armor;
                Console.WriteLine($"{source.Name} attacks {target.Name} {target.Health.Value}/{target.Health.MaxValue} for {hurt}.");
                if (hurt > 0)
                    target.Health.Sub(hurt);
            }
            return target;
        }

        public Creature Heal(Creature source, Creature target)
        {
            var heal = RollEffect(source.Intelligence.Value / 2, 3);
            Console.WriteLine($"{source.Name} heals {target.Name} {target.Health.Value}/{target.Health.MaxValue} for {heal}.");
            target.Health.Add(heal);
            return target;
        }

        public int RollEffect(int rollValue, int rollChance = -1)
        {
            if (rollChance > 0 && Dice.Roll() < rollChance)
            {
                return 0;
            }
            return Dice.Roll(rollValue);
        }
    }
}