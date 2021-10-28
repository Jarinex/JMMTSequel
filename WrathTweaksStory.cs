
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




namespace WrathTweakMod
{

    

    class WrathStoryTweaks
    {


        static internal void load()
        {

            //Test

            updateGiantFly();
            updateGiantFlyCopy();
            updateCultistEvoker();
            updateWightFighter();



        }


        //Test


        
        static void updateGiantFly()
        {

            var GiantFly = ResourcesLibrary.TryGetBlueprint<BlueprintUnit>("7e63418db0c4ec0428ab59c0947d628d");
            var displacement = ResourcesLibrary.TryGetBlueprint<BlueprintBuff>("00402bae4442a854081264e498e7a833");
            var capeofwasps = ResourcesLibrary.TryGetBlueprint<BlueprintAbility>("e418c20c8ce362943a8025d82c865c1c");
            var firefx = ResourcesLibrary.TryGetBlueprint<BlueprintBuff>("f36e0e6f50f241d40aecbbe145a7b436");
            var summonfirefly = ResourcesLibrary.TryGetBlueprint<BlueprintAbility>("1123e55a27974e4891cd96eb2b32271d");
            

            GiantFly.m_AddFacts = GiantFly.m_AddFacts.AddToArray(displacement.ToReference<BlueprintUnitFactReference>());
            GiantFly.m_AddFacts = GiantFly.m_AddFacts.AddToArray(summonfirefly.ToReference<BlueprintUnitFactReference>());



            GiantFly.Intelligence = 90;

            GiantFly.MaxHP = 9000;



            var brain = ResourcesLibrary.TryGetBlueprint<BlueprintBrain>("f5a012b8d0dab4f45924dcb2609df7b0");
            var summonfireflyai = ResourcesLibrary.TryGetBlueprint<BlueprintAiCastSpell>("1c33ba8f335d45c284dafe095854546c");
            brain.m_Actions = brain.m_Actions.AddToArray(summonfireflyai.ToReference<BlueprintAiActionReference>());

        }

        static void updateGiantFlyCopy()
        {

            var FireFly = ResourcesLibrary.TryGetBlueprint<BlueprintUnit>("f8bf325a6020435e9719f56a4651b440");
            var firefx = ResourcesLibrary.TryGetBlueprint<BlueprintBuff>("f36e0e6f50f241d40aecbbe145a7b436");
            var barkskin = ResourcesLibrary.TryGetBlueprint<BlueprintBuff>("533592a86adecda4e9fd5ed37a028432");

            FireFly.SetName("FireFly");


            FireFly.GetComponent<AddFacts>().m_Facts = FireFly.GetComponent<AddFacts>().m_Facts.AddToArray(firefx.ToReference<BlueprintUnitFactReference>());
            FireFly.GetComponent<AddFacts>().m_Facts = FireFly.GetComponent<AddFacts>().m_Facts.AddToArray(barkskin.ToReference<BlueprintUnitFactReference>());
            FireFly.GetComponent<AddFacts>().CasterLevel = 20;


        }

        static void updateCultistEvoker()
        {

            var CultistEvoker = ResourcesLibrary.TryGetBlueprint<BlueprintUnit>("46d14b326c3a8f549941ec2573ce0cd0");
            var displacement = ResourcesLibrary.TryGetBlueprint<BlueprintBuff>("00402bae4442a854081264e498e7a833");
            var displacementability = ResourcesLibrary.TryGetBlueprint<BlueprintAbility>("903092f6488f9ce45a80943923576ab3");
            var iceprisonability = ResourcesLibrary.TryGetBlueprint<BlueprintAbility>("65e8d23aef5e7784dbeb27b1fca40931");
            


            CultistEvoker.m_AddFacts = CultistEvoker.m_AddFacts.AddToArray(displacement.ToReference<BlueprintUnitFactReference>());

            CultistEvoker.MaxHP = 9000;



            CultistEvoker.Body.m_PrimaryHand = ResourcesLibrary.TryGetBlueprint<BlueprintItemWeapon>("c9e68ffc43bf8f349905e4c6cab539b8").ToReference<BlueprintItemEquipmentHandReference>();
            
            //CultistEvoker.GetComponent<AddClassLevels>().m_CharacterClass = ResourcesLibrary.TryGetBlueprint<BlueprintCharacterClass>("48ac8db94d5de7645906c7d0ad3bcfbd").ToReference<BlueprintCharacterClassReference>();
            //Above code changes classes



            CultistEvoker.GetComponent<AddClassLevels>().Levels = 15;
            CultistEvoker.GetComponent<Experience>().CR = 15;

            CultistEvoker.GetComponent<AddClassLevels>().m_SelectSpells = CultistEvoker.GetComponent<AddClassLevels>().m_SelectSpells.AddToArray<BlueprintAbilityReference>(displacementability.ToReference<BlueprintAbilityReference>());

            CultistEvoker.GetComponent<AddClassLevels>().m_SelectSpells = CultistEvoker.GetComponent<AddClassLevels>().m_SelectSpells.RemoveFromArray<BlueprintAbilityReference>(iceprisonability.ToReference<BlueprintAbilityReference>());

        }

        static void updateWightFighter()
        {

            var WightFigter = ResourcesLibrary.TryGetBlueprint<BlueprintUnit>("4ae78e4aa82d37a4cb6cbfbcdcc93195");
            var FighterClass = ResourcesLibrary.TryGetBlueprint<BlueprintCharacterClass>("48ac8db94d5de7645906c7d0ad3bcfbd");
            var UndeadClass = ResourcesLibrary.TryGetBlueprint<BlueprintCharacterClass>("19a2d9e58d916d04db4cd7ad2c7a3ee2");
            var dumbmonsterbrain = Resources.GetBlueprint<BlueprintBrain>("5abc8884c6f15204c8604cb01a2efbab");
            var capeofwasps = ResourcesLibrary.TryGetBlueprint<BlueprintAbility>("e418c20c8ce362943a8025d82c865c1c");

            WightFigter.m_AddFacts = WightFigter.m_AddFacts.AddToArray(capeofwasps.ToReference<BlueprintUnitFactReference>());

            WightFigter.GetComponents<AddClassLevels>()
            .Where(c => c.m_CharacterClass.Get() == FighterClass)
            .First().Levels = 7;

            var WightFighterBrain = Helpers.CreateCopy(dumbmonsterbrain, bp => {
                bp.AssetGuid = new BlueprintGuid(System.Guid.Parse("6cb77b6d51e84f04bbe644d69383a69c"));
                bp.name = "WightFighterBrain";
                
            });



            Resources.AddBlueprint(WightFighterBrain);

            WightFigter.m_Brain = ResourcesLibrary.TryGetBlueprint<BlueprintBrain>("6cb77b6d51e84f04bbe644d69383a69c").ToReference<BlueprintBrainReference>();


            var brain = ResourcesLibrary.TryGetBlueprint<BlueprintBrain>("5abc8884c6f15204c8604cb01a2efbab");
            var test = ResourcesLibrary.TryGetBlueprint<BlueprintAiCastSpell>("72a9d5d322d34304b06ddb68e9ce2063");
            brain.m_Actions = brain.m_Actions.AddToArray(test.ToReference<BlueprintAiActionReference>());

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
