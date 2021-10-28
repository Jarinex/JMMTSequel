
using dnlib.DotNet;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Experience;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Classes;
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
using UnityEngine;
using Kingmaker.PubSubSystem;
using Kingmaker;
using Kingmaker.EntitySystem.Entities;




namespace WrathTweakMod
{

    

    

    class WrathUnitCopy
    {




        static internal void load()
        {

            //Test

            updateGiantFlyCopy();




        }


        //Test



        static void updateGiantFlyCopy()
        {

            var GiantFly = ResourcesLibrary.TryGetBlueprint<BlueprintUnit>("7e63418db0c4ec0428ab59c0947d628d");


            var GiantFlyCopy = Helpers.CreateCopy(GiantFly, bp => {
                bp.AssetGuid = new BlueprintGuid(System.Guid.Parse("f8bf325a6020435e9719f56a4651b440"));
                bp.name = "Flaming Fly";
                bp.SetName("Flaming Fly");

            });

            Resources.AddBlueprint(GiantFlyCopy);






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
