
using dnlib.DotNet;
using Kingmaker.Blueprints;
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


   public class WrathUnitReplace

    {

      public struct UnitReplacement
        {
            public string originalBlueprint;
            public BlueprintUnit replacementBlueprint;
        }

        public class UnitReplacementAreaHandler : IAreaHandler
        {
            private Dictionary<string, BlueprintUnit> replacements = new();
            private string areaID;
            public void OnAreaDidLoad() //once the area is loaded, check for the original hydra
               try {
                    // Only do replacements if this is the correct area
                    if (Game.Instance.CurrentlyLoadedArea.AssetGuid.Equals(areaID))
                    {
                        // Go through all units in the area
                        foreach (var unit in Game.Instance.State.Units)
                        {
                            //if we are aupposed to replace this unit...
                            if (replacements.TryGetValue(unit.Blueprint.AssetGuid.ToString(), out string replacement))
                            {
                                // do it
                                unit.ReplaceWith(replacement);
                            }
    }
}
                } catch (Exception ex)
{
    Main.DebugError(ex);
}


public void OnAreaBeginUnloading()
            {

            }

            public UnitReplacementAreaHandler(string area, UnitReplacement[] replacementsArray)
            {
                this.areaID = area;
                foreach (var replace in replacementsArray)
                    replacements[replace.originalBlueprint] = replace.replacementBlueprint;

            }

        }



        public static void InstallUnitReplacer(string area, params UnitReplacement[] replacements)
        {
            EventBus.Subscribe(new UnitReplacementAreaHandler(area, replacements));
        }

        public static string Test = "cd057d4d28be9fc46949fa1148bece7f";
        public static string Firefly = "f8bf325a6020435e9719f56a4651b440";
        public static string caves = " bad465e5404604346bdf71ef50b3299a";
       

        internal static void load()
        {
            InstallUnitReplacer(caves, new UnitReplacement
            {
                originalBlueprint = Test,
                replacementBlueprint = Resources.GetBlueprint<BlueprintUnit>(Firefly)
            });
              
        }
    }


    public static class Jari
    {
        public static void ReplaceWith(this UnitEntityData toReplace, BlueprintUnit replaceWith)
        {
            var newUnit = Game.Instance.EntityCreator.SpawnUnit(replaceWith, toReplace.Position, Quaternion.LookRotation(toReplace.OrientationDirection), Game.Instance.State.LoadedAreaState.MainState);
            newUnit.GroupId = toReplace.GroupId;
            toReplace.Destroy();
        }
    }

}

