using BattleTech;
using BattleTech.Save;
using BattleTech.Save.SaveGameStructure;
using BattleTech.UI;
using Harmony;
using System.Collections.Generic;
using System.Reflection;

namespace nl.flukeyfiddler.bt.IronMechMode.Util.Debug
{
    [HarmonyPatch(typeof(MainMenu), "HandleEscapeKeypress")]
    public class MainMenu_HandleEscapeKeyPress_Patch_Debug
    {
        public static void Postfix(MainMenu __instance)
        {
        }
    }

    [HarmonyPatch(typeof(GameInstance), "CanSave")]
    public class GameInstance_Save_Patch_Debug
    {
        static void Postfix(GameInstance __instance, SaveReason reason, bool __result)
        {
            if (!__result)
            {
                // DebugHelper.LogGameInstanceCanSave(__instance, reason);
            }
        }
    }

    [HarmonyPatch(typeof(CombatGameState), "CanSave")]
    public class CombatGameState_Save_Patch_Debug
    {
        static void Postfix(CombatGameState __instance, bool __result)
        {
            if (!__result)
            {
                DebugHelper.LogCombatGameStateCanSave(__instance);
            }
        }
    }

    [HarmonyPatch(typeof(ReasonToSlotGroupMapping), "GetSlotGroup")]
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

    [HarmonyPatch(typeof(SimGameState), "Dehydrate")]
    public class SimGameState_Dehydrate_Patch_Debug
    {
        public static void Prefix(SimGameState __instance, SimGameSave save,
            List<Contract> ___globalContracts, List<ContractData> ___contractBits)
        {
            List<string> debugLines = new List<string>();
            debugLines.Add("Dehydrate");
            debugLines.Add("Active contract name: " + __instance.SaveActiveContractName);
            debugLines.Add("global contracts: " + ___globalContracts.Count);
            debugLines.Add("contract bits: " + ___contractBits.Count);
            Logger.Block(debugLines.ToArray(), MethodBase.GetCurrentMethod());
        }
    }
}


/*
 * BattleTech.Save.SaveGameStructure.Messages
 * BattleTech.CombatGameState.CanSave
 * BattleTech.SimGameState.CanSave
 * BattleTech.CombatGameState.Update() if F10 pressed
 * BattleTech.CombatGameState.TriggerAutosaving
 * BattleTech.Save.SaveGameStructure.SlotModel.GetDisplayText()
 * BattleTech.Save.SaveGameStructure.SlotGrouping(int maxSaves, SlotGroupFullBehaviour behaviour)
 * BattleTech.Save.SaveGameStructure.SlotGroupingParametersMapping maxSaves is set here
 * 
 * classtype BattleTech.Save.SaveGameStructure.SaveReason
 * SlotGroup.AutoSaves_1,
 * BattleTech.UI.SimGameOptionsMenu.ReceiveButtonPress -->
 */
