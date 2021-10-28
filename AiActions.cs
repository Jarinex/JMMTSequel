
using dnlib.DotNet;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Experience;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.Items.Equipment;
using Kingmaker.Blueprints.Items;
using Kingmaker.Blueprints.Items.Shields;
using Kingmaker.Blueprints.Items.Armors;
using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.Visual;
using Kingmaker.Visual.Sound;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.Localization;
using Kingmaker.ResourceLinks;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.AI.Blueprints.Considerations;
using Kingmaker.AI.Blueprints.Considerations;
using Kingmaker.AI.Blueprints;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using HarmonyLib;
using Kingmaker.View;
using System;
using System.Collections.Generic;
using System.Linq;








namespace WrathTweakMod
{



    class Test
    {


        static internal void load()
        {

            //Test

            AddCapeofWaspsAiAction();
            AddSummonFireFly();

        }

        public static void AddSummonFireFly()
        {
            var fireflysummon = ResourcesLibrary.TryGetBlueprint<BlueprintAbility>("1123e55a27974e4891cd96eb2b32271d");
            var Castfireflysummon = Helpers.Create<BlueprintAiCastSpell>(bp =>
            {
                bp.name = "SummonFireflyAi";
                bp.m_Ability = fireflysummon.ToReference<BlueprintAbilityReference>();
               
                bp.CombatCount = 1;
                bp.BaseScore = 50;
                bp.AssetGuid = new BlueprintGuid(System.Guid.Parse("1c33ba8f335d45c284dafe095854546c"));
            });
            Resources.AddBlueprint(Castfireflysummon);
        }


            public static void AddCapeofWaspsAiAction()
        {
            var capeofwasps = ResourcesLibrary.TryGetBlueprint<BlueprintAbility>("e418c20c8ce362943a8025d82c865c1c");
            var CastCapeofWasps = Helpers.Create<BlueprintAiCastSpell>(bp =>
            {
                bp.name = "Test";
                bp.m_Ability = capeofwasps.ToReference<BlueprintAbilityReference>();
                bp.m_ForceTargetSelf = true;
                bp.CombatCount = 1;
                bp.BaseScore = 50;
                bp.AssetGuid = new BlueprintGuid(System.Guid.Parse("72a9d5d322d34304b06ddb68e9ce2063"));
            });
            Resources.AddBlueprint(CastCapeofWasps);


        }
    }
}




























//public override string ToString()
//  {
//      return base.ToString();
//   }

//   public override bool Equals(object obj)
//   {
//     return base.Equals(obj);
//  }

//  public override int GetHashCode()
//   {
//      return base.GetHashCode();
//   }
//   }

//}