using BattleTech;
using BattleTech.Save.SaveGameStructure;
using BattleTech.UI;
using Harmony;
using nl.flukeyfiddler.bt.IronMechMode.Util;
using nl.flukeyfiddler.bt.IronMechMode.Util.Debug;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace nl.flukeyfiddler.bt.IronMechMode
{
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
            /*
            bool newRoundInBattle = __instance.CurrentPhase == __instance.FirstPhase;
            // When not in a battle, the currentPhase is always 5 while first is 1
            bool newRoundOutsideOfBattle = __instance.CurrentPhase == __instance.LastPhase;
           */
            bool playerTeamTurn = __instance.ActiveTurnActor.GUID == __instance.Combat.LocalPlayerTeamGuid;

            if (playerTeamTurn && __instance.CurrentRound > lastRound)
            {
                __instance.Combat.BattleTechGame.Save(ModSettings.COMBAT_AUTOSAVE_REASON, false);
                SetRound(__instance.CurrentRound);
            }
        }
    }

    [HarmonyPatch(typeof(SlotModel), "_GetDisplayText")]
    public class  SlotModel_GetDisplayText_Patch
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
            foreach(KeyValuePair<SaveReason, SlotGroup> _override in ModSettings.modAutosaveMapping)
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