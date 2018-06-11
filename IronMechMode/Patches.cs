using BattleTech;
using BattleTech.Save.SaveGameStructure;
using BattleTech.UI;
using Harmony;
using nl.flukeyfiddler.bt.IronMechMode.Util;
using nl.flukeyfiddler.bt.IronMechMode.Util.Debug;
using nl.flukeyfiddler.bt.Utils;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace nl.flukeyfiddler.bt.IronMechMode
{
    [HarmonyPatch(typeof(SimGameState))]
    public class SimGameState_Ctor_Patch
    {
        public static void Postfix(SimGameState __instance)
        {
            Logger.Line("in SimGameState Ctor");
            ModSettings.InstantiateAutoSavePatches(__instance);
            
            foreach (object classInstance in ModSettings.AutosavePatches.Keys)
            {
                foreach (KeyValuePair<MethodName, ShouldSaveCondition> patchParams in ModSettings.AutosavePatches[classInstance])
                {
                    if (patchParams.Value.ShouldSave(classInstance))
                    {
                        MethodInfo targetMethod = Traverse.Create(classInstance).GetType().GetMethod(patchParams.Key.ToString());
                        Logger.Line("targetMethod name: " + targetMethod.Name);
                        MethodInfo patchMethod = typeof(Helper).GetMethod("TriggerSimGameAutosave");
                        Logger.Line("mehthod of patch class: " + patchMethod.Name);

                        Logger.Line(String.Format("should patch {0} using {1}", targetMethod.Name, patchMethod.Name));

                        IronMechMode.harmony.Patch(targetMethod, null, new HarmonyMethod(patchMethod));
                    }
                }
            }
        }
    }

    [HarmonyPatch(typeof(GameInstance), "CanSave")]
    public class GameInstance_CanSave_Patch
    {
        static void Postfix(GameInstance __instance, ref bool __result, SaveReason reason)
        {
            if (reason == SaveReason.MANUAL)
            {
                __result = false;
            }
        }
    }

    [HarmonyPatch(typeof(TurnDirector), "IncrementActiveTurnActor")]
    public static class TurnDirector_IncrementActiveTurnActor_Patch
    {
        public static int lastRound = 0;

        public static void SetRound(int currentRound)
        {
            lastRound = currentRound;
        }

        private static void Postfix(TurnDirector __instance)
        {
            bool playerTeamTurn = __instance.ActiveTurnActor.GUID == __instance.Combat.LocalPlayerTeamGuid;

            if (playerTeamTurn && __instance.CurrentRound > lastRound)
            {
                __instance.Combat.BattleTechGame.Save(ModSettings.COMBATGAME_AUTOSAVE_REASON, false);
                SetRound(__instance.CurrentRound);
            }
        }
    }

    [HarmonyPatch(typeof(SlotModel), "_GetDisplayText")]
    public class SlotModel_GetDisplayText_Patch
    {
        static bool Prefix(SlotModel __instance, ref string __result)
        {
            string debugSaveReason = __instance.SaveReason.ToString();
            __result = __instance.SaveReason.ToString();
            return false;
        }
    }

    [HarmonyPatch(typeof(ReasonToSlotGroupMapping), "GetSlotGroup")]
    public class ReasonToSlotGroupMapping_GetSlotGroup_Patch
    {
        static void Prefix(SaveReason reason, ref Dictionary<SaveReason, SlotGroup> ___mapping)
        {
            foreach (KeyValuePair<SaveReason, SlotGroup> _override in ModSettings.AutosaveMapping)
            {
                ___mapping[_override.Key] = _override.Value;
            }
        }
    }

    [HarmonyPatch(typeof(SlotGroupingParametersMapping), "GetSlotGroupings")]
    public class SlotGroupingParametersMapping_Patch
    {
        public static void Postfix(ref Dictionary<SlotGroup, SlotGrouping> __result)
        {
            __result[ModSettings.AUTOSAVES_GROUP] = new SlotGrouping(ModSettings.settings.MaxAutoSaves, SlotGroupFullBehavior.AUTO_OVERWRITE_OLDEST);
            __result[ModSettings.CHECKPOINTSAVES_GROUP] = new SlotGrouping(ModSettings.settings.MaxCheckpointSaves, SlotGroupFullBehavior.AUTO_OVERWRITE_OLDEST);
        }
    }
}