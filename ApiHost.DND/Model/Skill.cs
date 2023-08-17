using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiHost.DND.Model
{
    public class Skill : StatusVolume
    {
        public Skill(int id, string name, ActionType action, ElementType element, EffectType effect, int manaCost, int staminaCost, int skillRange, bool isAOE, int aOERange, bool isEquipEffect,int value,int maxValue)
        {
            ID = id;
            Name = name;
            ActionType = action;
            ElementType = element;
            EffectType = effect;
            ManaCost = manaCost;
            StaminaCost = staminaCost;
            SkillRange = skillRange;
            IsAOE = isAOE;
            AOERange = aOERange;
            IsEquipEffect = isEquipEffect;
            Value = value;
            MaxValue = maxValue;
        }

        public ActionType ActionType { get; set; }
        public ElementType ElementType { get; set; }
        public EffectType EffectType { get; set; }
        public int ManaCost { get; set; }
        public int StaminaCost { get; set; }
        public int SkillRange { get; set; }
        public bool IsAOE { get; set; }
        public int AOERange { get; set; }
        public bool IsEquipEffect { get; set; }
    }
}
