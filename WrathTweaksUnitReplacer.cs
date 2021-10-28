
using dnlib.DotNet;
using Kingmaker.Blueprints;
using Kingmaker.View;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Kingmaker.PubSubSystem;
using Kingmaker;





namespace WrathTweakMod
{


    class WrathUnitReplace

    {

        struct UnitReplacement
        {
            string originalBlueprint;
            BlueprintUnit replacementBlueprint;
        }

        public class UnitReplacementAreaHandler : IAreaHandler
        {
            private Dictionary<string, BlueprintUnit> replacements = new();
            private string areaID;
            public void OnAreaLoadingComplete() //once the area is loaded, check for the original hydra
            {
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
            }

            public UnitReplacementAreaHandler(string area, UnitReplacement[] replacementsArray)
            {
                this.areaID = area;
                foreach (var replace in replacementsArray)
                    replacements[replace.originalBlueprint] = replace.replacementBlueprint;

            }

        }

        public static void ReplaceWith(this UnitEntityData toReplace, BlueprintUnit replaceWith)
        {
            var newUnit = Game.Instance.EntityCreator.SpawnUnit(replaceWith, toReplace.Position, Quaternion.LookRotation(toReplace.OrientationDirection), Game.Instance.CurrentScene.MainState);
            newunit.GroupId = toReplace.GroupId;
            toReplace.Destroy();
        }

        public static void InstallUnitReplacer(string area, params UnitReplacement[] replacements)
        {
            EventBus.Subscribe(new UnitReplacementAreaHandler(area, replacements));
        }


    }
}
