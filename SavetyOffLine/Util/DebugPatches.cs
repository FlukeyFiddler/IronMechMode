using BattleTech;
using BattleTech.Save.SaveGameStructure;
using BattleTech.UI;
using Harmony;
using System.Collections.Generic;
using System.Reflection;

namespace nl.flukeyfiddler.bt.SavetyOffLine.Util.Debug
{
    [HarmonyPatch(typeof(MainMenu), "HandleEscapeKeypress")]
    [HarmonyPriority(Priority.Last)]
    public class MainMenu_HandleEscapeKeyPress_Patch_Debug
    {
        public static void Postfix(MainMenu __instance)
        {
        }
    }

    [HarmonyPatch(typeof(CombatGameState), "CanSave")]
    [HarmonyPriority(Priority.Last)]
    public class CombatGameState_Save_Patch_Debug
    {
        public static void Postfix(CombatGameState __instance, bool __result)
        {
            if (!__result)
            {
                DebugHelper.LogCombatGameStateCanSave(__instance);
            }
        }
    }

    [HarmonyPatch(typeof(SimGameState), "OnHeadlessCompleteListner")]
    [HarmonyPriority(Priority.Last)]
    public class SimGameState_OnHeadlessCompleteListner_Patch_Debug
    {
        static List<string> debugLines = new List<string>();

        private static void Postfix(SimGameState __instance)
        {

            var isIronManCampaign = Traverse.Create(__instance).Property("IsIronmanCampaign");

            debugLines.Add("Checking for Ironman");
            debugLines.Add("Field exists: " + isIronManCampaign.FieldExists());

            if (isIronManCampaign.FieldExists())
            {
                debugLines.Add("Is ironMan Campaign: " + isIronManCampaign.GetValue<bool>());

                if (isIronManCampaign.GetValue<bool>())
                {
                    debugLines.Add("Patches after removal: ");
                    SavetyOffLine.harmony.GetPatchedMethods().Do(getPatchedMethods);
                }
                else
                {
                    debugLines.Add("Not Ironman, patches:");
                    SavetyOffLine.harmony.GetPatchedMethods().Do(getPatchedMethods);
                }
            }
            Logger.Block(debugLines.ToArray(), MethodBase.GetCurrentMethod());
            debugLines.Clear();
        }

        private static void getPatchedMethods(MethodBase method)
        {
            SavetyOffLine.harmony.GetPatchInfo(method).Owners.Do(delegate (string owner) {
                debugLines.Add("Patch: " + method.Name);
                debugLines.Add("  owner: " + owner);
            });
        }
    }

    [HarmonyPatch(typeof(ReasonToSlotGroupMapping), "GetSlotGroup")]
    [HarmonyPriority(Priority.Last)]
    public class ReasonToSlotGroupMapping_GetSlotGroup_Patch_Debug
    {
        public static void Postfix(SaveReason reason, SlotGroup __result)
        {
            if (ModSettings.AutosaveMapping.ContainsKey(reason))
            {
                if (ModSettings.AutosaveMapping[reason] != __result)
                {
                    Logger.InfoLine(MethodBase.GetCurrentMethod());
                    Logger.Minimal("Reason: " + reason);
                    Logger.Minimal("SlotGroup expected: " + ModSettings.AutosaveMapping[reason]);
                    Logger.Minimal("SlotGroup Actual: " + __result);
                    Logger.EndLine();
                }
            }
        }
    }

    [HarmonyPatch(typeof(TurnDirector), "IncrementActiveTurnActor")]
    [HarmonyPriority(Priority.Last)]
    public class TurnDirector_IncrementActiveTurnActor_Patch_Debug
    {
        private static void Prefix(TurnDirector __instance)
        {
            bool playerTeamTurn = __instance.ActiveTurnActor.GUID == __instance.Combat.LocalPlayerTeamGuid;

            if (playerTeamTurn && (TurnDirector_IncrementActiveTurnActor_Patch.lastRound > __instance.CurrentRound))
            {
                Logger.InfoLine(MethodBase.GetCurrentMethod());
                Logger.Minimal("Should try to save");
                Logger.EndLine();
            }
        }
    }      
}