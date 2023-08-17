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
        public string source { get; set; }
        public string target { get; set; }
        public ActionType action { get; set; }
    }

    public class CreatureRequest : BaseMessageContent
    {
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
    }
}
