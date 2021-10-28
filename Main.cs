
using UnityModManagerNet;
using System;
using System.Reflection;
using Kingmaker.Blueprints;
using System.Collections.Generic;
using HarmonyLib;
using Kingmaker.ElementsSystem;
using Kingmaker.Blueprints.JsonSystem;
using JetBrains.Annotations;
using System.Linq;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Components;
using dnlib.DotNet;
using Kingmaker.Localization;
using Kingmaker.Blueprints.Facts;
using Kingmaker.PubSubSystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.Controllers;
using Kingmaker.AI.Blueprints;
using Kingmaker;



namespace WrathTweakMod
{

  
    static class Resources
    {
        public static readonly Dictionary<BlueprintGuid, SimpleBlueprint> ModBlueprints = new Dictionary<BlueprintGuid, SimpleBlueprint>();
#if false
        public static IEnumerable<T> GetBlueprints<T>() where T : BlueprintScriptableObject {
            if (blueprints == null) {
                var bundle = ResourcesLibrary.s_BlueprintsBundle;
                blueprints = bundle.LoadAllAssets<BlueprintScriptableObject>();
                //blueprints = Kingmaker.Cheats.Utilities.GetScriptableObjects<BlueprintScriptableObject>();
            }
            return blueprints.Concat(ResourcesLibrary.s_LoadedBlueprints.Values).OfType<T>().Distinct();
        }
#endif
        public static T GetBlueprint<T>(string id) where T : SimpleBlueprint
        {
            var assetId = new BlueprintGuid(System.Guid.Parse(id));
            return GetBlueprint<T>(assetId);
        }
        public static T GetBlueprint<T>(BlueprintGuid id) where T : SimpleBlueprint
        {
            SimpleBlueprint asset = ResourcesLibrary.TryGetBlueprint(id);
            T value = asset as T;
            if (value == null) { Main.Error($"COULD NOT LOAD: {id} - {typeof(T)}"); }
            return value;

        }
        public static void AddBlueprint([NotNull] BlueprintScriptableObject blueprint)
        {
            AddBlueprint(blueprint, blueprint.AssetGuid);
        }
        public static void AddBlueprint([NotNull] SimpleBlueprint blueprint, string assetId)
        {
            var Id = new BlueprintGuid(System.Guid.Parse(assetId));
            AddBlueprint(blueprint, Id);
        }
        public static void AddBlueprint([NotNull] SimpleBlueprint blueprint, BlueprintGuid assetId)
        {
            var loadedBlueprint = ResourcesLibrary.TryGetBlueprint(assetId);
            if (loadedBlueprint == null)
            {
                ModBlueprints[assetId] = blueprint;
                ResourcesLibrary.BlueprintsCache.AddCachedBlueprint(assetId, blueprint);
                blueprint.OnEnable();

            }

        }
    }

    public static class Helpers


    {



        public static T Create<T>(Action<T> init = null) where T : new()
        {
            var result = new T();
            init?.Invoke(result);
            return result;
        }

        public static void SetComponents(this BlueprintScriptableObject obj, params BlueprintComponent[] components)
        {
            // Fix names of components. Generally this doesn't matter, but if they have serialization state,
            // then their name needs to be unique.
            var names = new HashSet<string>();
            foreach (var c in components)
            {
                if (string.IsNullOrEmpty(c.name))
                {
                    c.name = $"${c.GetType().Name}";
                }
                if (!names.Add(c.name))
                {

                    String name;
                    for (int i = 0; !names.Add(name = $"{c.name}${i}"); i++) ;
                    c.name = name;
                }

            }

            obj.ComponentsArray = components;
        }

        public static T CreateCopy<T>(T original, Action<T> init = null)
        {
            var result = (T)ObjectDeepCopier.Clone(original);
            init?.Invoke(result);
            return result;
        }

        public static List<TSource> ToList<TSource>(this IEnumerable<TSource> source)
        {
            
            return new List<TSource>(source);
        }






        public static T[] RemoveFromArray<T>(this T[] array, T value)
        {
            var list = array.ToList();
            return list.Remove(value) ? list.ToArray() : array;
        }

        public static ActionList CreateActionList(params GameAction[] actions)
        {
            if (actions == null || actions.Length == 1 && actions[0] == null) actions = Array.Empty<GameAction>();
            return new ActionList() { Actions = actions };
        }

     


        public static AbilityEffectRunAction CreateRunActions(params GameAction[] actions)
         {
            var result = Create<AbilityEffectRunAction>();
             result.Actions = CreateActionList(actions);
            return result;
        }

        public static T[] AppendToArray<T>(this T[] array, T value)
        {
            var len = array.Length;
            var result = new T[len + 1];
            Array.Copy(array, result, len);
            result[len] = value;
            return result;
        }

        // All localized strings created in this mod, mapped to their localized key. Populated by CreateString.
        static Dictionary<String, LocalizedString> textToLocalizedString = new Dictionary<string, LocalizedString>();
        public static LocalizedString CreateString(string key, string value)
        {
            // See if we used the text previously.
            // (It's common for many features to use the same localized text.
            // In that case, we reuse the old entry instead of making a new one.)
            LocalizedString localized;
            if (textToLocalizedString.TryGetValue(value, out localized))
            {
                return localized;
            }
            var strings = LocalizationManager.CurrentPack.Strings;

            strings[key] = value;
            localized = new LocalizedString
            {
                m_Key = key
            };
            textToLocalizedString[value] = localized;
            return localized;
        }




        public static void SetNameDescription(this BlueprintUnitFact feature, BlueprintUnitFact other)
        {
            feature.m_DisplayName = other.m_DisplayName;
            feature.m_Description = other.m_Description;
        }

        public static void SetName(this BlueprintUnitFact feature, LocalizedString name)
        {
            feature.m_DisplayName = name;
        }

        public static void SetName(this BlueprintUnitFact feature, String name)
        {
            feature.m_DisplayName = Helpers.CreateString(feature.name + ".Name", name);
        }

        public static void SetDescriptionUntagged(this BlueprintUnitFact feature, String description)
        {
            feature.m_Description = Helpers.CreateString(feature.name + ".Description", description);
        }

        public static void SetDescription(this BlueprintUnitFact feature, LocalizedString description)
        {
            feature.m_Description = description;
            //blueprintUnitFact_set_Description(feature) = description;
        }

        

        public static void AddComponent(this BlueprintScriptableObject obj, BlueprintComponent component)
        {
            obj.SetComponents(obj.ComponentsArray.AppendToArray(component));
        }


        private class ObjectDeepCopier
        {
            private class ArrayTraverse
            {
                public int[] Position;
                private int[] maxLengths;

                public ArrayTraverse(Array array)
                {
                    maxLengths = new int[array.Rank];
                    for (int i = 0; i < array.Rank; ++i)
                    {
                        maxLengths[i] = array.GetLength(i) - 1;
                    }
                    Position = new int[array.Rank];
                }

                public bool Step()
                {
                    for (int i = 0; i < Position.Length; ++i)
                    {
                        if (Position[i] < maxLengths[i])
                        {
                            Position[i]++;
                            for (int j = 0; j < i; j++)
                            {
                                Position[j] = 0;
                            }
                            return true;
                        }
                    }
                    return false;
                }
            }
            private class ReferenceEqualityComparer : EqualityComparer<Object>
            {
                public override bool Equals(object x, object y)
                {
                    return ReferenceEquals(x, y);
                }
                public override int GetHashCode(object obj)
                {
                    if (obj == null) return 0;
                    return obj.GetHashCode();
                }
            }
            private static readonly MethodInfo CloneMethod = typeof(Object).GetMethod("MemberwiseClone", BindingFlags.NonPublic | BindingFlags.Instance);

            public static bool IsPrimitive(Type type)
            {
                if (type == typeof(String)) return true;
                return (type.IsValueType & type.IsPrimitive);
            }
            public static Object Clone(Object originalObject)
            {
                return InternalCopy(originalObject, new Dictionary<Object, Object>(new ReferenceEqualityComparer()));
            }
            private static Object InternalCopy(Object originalObject, IDictionary<Object, Object> visited)
            {
                if (originalObject == null) return null;
                var typeToReflect = originalObject.GetType();
                if (IsPrimitive(typeToReflect)) return originalObject;
                if (visited.ContainsKey(originalObject)) return visited[originalObject];
                if (typeof(Delegate).IsAssignableFrom(typeToReflect)) return null;
                var cloneObject = CloneMethod.Invoke(originalObject, null);
                if (typeToReflect.IsArray)
                {
                    var arrayType = typeToReflect.GetElementType();
                    if (IsPrimitive(arrayType) == false)
                    {
                        Array clonedArray = (Array)cloneObject;
                        ForEach(clonedArray, (array, indices) => array.SetValue(InternalCopy(clonedArray.GetValue(indices), visited), indices));
                    }

                }
                visited.Add(originalObject, cloneObject);
                CopyFields(originalObject, visited, cloneObject, typeToReflect);
                RecursiveCopyBaseTypePrivateFields(originalObject, visited, cloneObject, typeToReflect);
                return cloneObject;

                void ForEach(Array array, Action<Array, int[]> action)
                {
                    if (array.LongLength == 0) return;
                    ArrayTraverse walker = new ArrayTraverse(array);
                    do action(array, walker.Position);
                    while (walker.Step());
                }
            }
            private static void RecursiveCopyBaseTypePrivateFields(object originalObject, IDictionary<object, object> visited, object cloneObject, Type typeToReflect)
            {
                if (typeToReflect.BaseType != null)
                {
                    RecursiveCopyBaseTypePrivateFields(originalObject, visited, cloneObject, typeToReflect.BaseType);
                    CopyFields(originalObject, visited, cloneObject, typeToReflect.BaseType, BindingFlags.Instance | BindingFlags.NonPublic, info => info.IsPrivate);
                }
            }
            private static void CopyFields(object originalObject, IDictionary<object, object> visited, object cloneObject, Type typeToReflect, BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.FlattenHierarchy, Func<FieldInfo, bool> filter = null)
            {
                foreach (FieldInfo fieldInfo in typeToReflect.GetFields(bindingFlags))
                {
                    if (filter != null && filter(fieldInfo) == false) continue;
                    if (IsPrimitive(fieldInfo.FieldType)) continue;
                    var originalFieldValue = fieldInfo.GetValue(originalObject);
                    var clonedFieldValue = InternalCopy(originalFieldValue, visited);
                    fieldInfo.SetValue(cloneObject, clonedFieldValue);
                }
            }
        }
        public delegate ref S FastRef<T, S>(T source = default);
        public delegate void FastSetter<T, S>(T source, S value);
        public delegate S FastGetter<T, S>(T source);
        public delegate object FastInvoke(object target, params object[] paramters);
    


        public static void ReplaceComponent(this BlueprintScriptableObject obj, BlueprintComponent original, BlueprintComponent replacement)
        {
  

            var components = obj.ComponentsArray;
        var newComponents = new BlueprintComponent[components.Length];
            for (int i = 0; i<components.Length; i++)
            {
                var c = components[i];
        newComponents[i] = c == original? replacement : c;
            }
         obj.SetComponents(newComponents); 
                 }


        //  public static ActionList CreateActionList(params GameAction[] actions)
        //  {
        //       if (actions == null || actions.Length == 1 && actions[0] == null) actions = Array.Empty<GameAction>();
        //       return new ActionList() { Actions = actions };
        //   }


        //    public static void AddAction(this Kingmaker.UnitLogic.Abilities.Components.AbilityEffectRunAction action, Kingmaker.ElementsSystem.GameAction game_action)
        //  {
        //      if (action.Actions != null)
        //      {
        //         action.Actions = Helpers.CreateActionList(action.Actions.Actions);
        //         action.Actions.Actions = action.Actions.Actions.AddToArray(game_action);
        //     }
        //     else
        //    {
        //        action.Actions = Helpers.CreateActionList(game_action);
        //    }
        // }

        //  public static BlueprintAbility CreateAbility(string name, string displayName, string description, string guid, Sprite icon, AbilityType type, UnitCommand.CommandType actionType, AbilityRange range, string duration, string savingThrow, params ActionList[] components);


    }



        internal class Main
        {

            internal static UnityModManagerNet.UnityModManager.ModEntry.ModLogger logger;
            internal static HarmonyLib.Harmony harmony;
            internal static BlueprintScriptableObject Library;

        static readonly Dictionary<Type, bool> typesPatched = new Dictionary<Type, bool>();
            static readonly List<String> failedPatches = new List<String>();
            static readonly List<String> failedLoading = new List<String>();

            [System.Diagnostics.Conditional("DEBUG")]
            internal static void DebugLog(string msg)
            {
                if (logger != null) logger.Log(msg);
            }
            internal static void DebugError(Exception ex)
            {
                if (logger != null) logger.Log(ex.ToString() + "\n" + ex.StackTrace);
            }
            internal static bool enabled;


            static bool Load(UnityModManager.ModEntry modEntry)
            {
                try
                {
                    logger = modEntry.Logger;
                    var harmony = new Harmony(modEntry.Info.Id);
                    harmony.PatchAll(Assembly.GetExecutingAssembly());


                }
                catch (Exception ex)
                {
                    DebugError(ex);
                    throw ex;
                }
                return true;
            }

            [HarmonyLib.HarmonyPatch(typeof(BlueprintsCache), "Init")]

            static class WrathTweaks



            {
                [HarmonyLib.HarmonyBefore(new string[] { "WrathTweakMod" })]

                static void Postfix()
                {

                    try
                    {
                        Main.logger.Log("Loading Wrath Tweak Mod");

                    WrathUnitCopy.load();
                    WrathUnitReplace.load();
                    WrathSpellsTweaks.load();
                    Test.load();
                    WrathStoryTweaks.load();



                }


                    catch (Exception ex)
                    {
                        Main.DebugError(ex);
                    }
                }
            }

            internal static Exception Error(String message)
            {
                logger?.Log(message);
                return new InvalidOperationException(message);
            }
        }

    }







    

    






