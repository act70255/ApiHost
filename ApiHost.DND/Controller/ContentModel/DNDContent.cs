using ApiHost.DND.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiHost.DND.Controller
{
    public class ActionRequest : BaseMessageContent
    {
        public int source { get; set; }
        public int target { get; set; }
        public int skill { get; set; }
    }
    public class SkillsRequest : BaseMessageContent
    {
        public List<int> ids { get; set; }
    }

    public class CreatureRequest : BaseMessageContent
    {
        public CreatureRequest() { }
        public CreatureRequest(string name, int healthValue = 10, int manaValue = 10, int staminaValue = 10, int experienceValue = 10, int levelValue = 10, int armorClassValue = 10, int attackBonusValue = 10, int damageValue = 10, int strengthValue = 10, int dexterityValue = 10, int intelligenceValue = 10, int charismaValue = 10, params int[] skills)
        {
            Name = name;
            HealthValue = healthValue;
            ManaValue = manaValue;
            StaminaValue = staminaValue;
            ExperienceValue = experienceValue;
            LevelValue = levelValue;
            ArmorClassValue = armorClassValue;
            AttackBonusValue = attackBonusValue;
            DamageValue = damageValue;
            StrengthValue = strengthValue;
            DexterityValue = dexterityValue;
            IntelligenceValue = intelligenceValue;
            CharismaValue = charismaValue;
            Skills = skills;
        }
        public int ID { get; set; }
        public string Name { get; set; }
        public int HealthValue { get; set; }
        public int ManaValue { get; set; }
        public int StaminaValue { get; set; }
        public int ExperienceValue { get; set; }
        public int LevelValue { get; set; }
        public int ArmorClassValue { get; set; }
        public int AttackBonusValue { get; set; }
        public int DamageValue { get; set; }
        public int StrengthValue { get; set; }
        public int DexterityValue { get; set; }
        public int IntelligenceValue { get; set; }
        public int CharismaValue { get; set; }
        public int[] Skills { get; set; }
    }
}
