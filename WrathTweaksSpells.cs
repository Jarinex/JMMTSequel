
using dnlib.DotNet;
using Kingmaker.Blueprints;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TinyJson;
using UnityEngine.UI;
using Kingmaker.EntitySystem.Entities;
using UnityEngine;
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
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;





namespace Kingmaker.UnitLogic.Mechanics.Actions
{


    public class ContextActionSpawnMonster : ContextAction
    {

        public override string GetCaption()
        {
           return "Summon monster";

        }

        public override void RunAction()
       {
            UnitEntityData maybeCaster = base.Context.MaybeCaster;
            if (maybeCaster == null)
            {
               PFLog.Default.Log(this, "Caster is missing", Array.Empty<object>());
                return;
            }
            Vector3 vector = base.Target.Point;
            vector += new Vector3(3, 0, -2);
           vector = ObstacleAnalyzer.GetNearestNode(vector).clampedPosition;
            UnitEntityView unitEntityView = this.Blueprint.Prefab.Load(false);
            float radius = (unitEntityView != null) ? unitEntityView.Corpulence : 0.5f;
           FreePlaceSelector.PlaceSpawnPlaces(3, radius, vector);
           Game.Instance.EntityCreator.SpawnUnit(this.Blueprint, vector, Quaternion.identity, maybeCaster.HoldingState);
        }

        public BlueprintUnit Blueprint;
   }
}



namespace WrathTweakMod
{
    class WrathSpellsTweaks
    {

       
   
      static internal void load()
       {
           summonfly();


       }



        public static void summonfly()
        {
            var flamingfly = ResourcesLibrary.TryGetBlueprint<BlueprintUnit>("f8bf325a6020435e9719f56a4651b440");

            var testactions = Helpers.CreateRunActions(
               Helpers.Create<ContextActionSpawnMonster>(c => c.Blueprint = flamingfly));

            var summonfly = Helpers.Create<BlueprintAbility>( bp =>
            {
                bp.name= "Summon Fly";
                bp.SetName("Summon Fly");

                bp.Animation = Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Omni;
                bp.ActionType = Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Swift;
                bp.m_IsFullRoundAction = false;
                bp.Range = AbilityRange.Close;
                bp.AddComponent(testactions);

                bp.AssetGuid = new BlueprintGuid(System.Guid.Parse("1123e55a27974e4891cd96eb2b32271d"));
            });
            Resources.AddBlueprint(summonfly);
        }





        public override string ToString()
        {
            return base.ToString();
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

}