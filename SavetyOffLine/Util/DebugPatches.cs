using BattleTech;
using BattleTech.Save.SaveGameStructure;
using BattleTech.UI;
using Harmony;
using System.Reflection;

namespace nl.flukeyfiddler.bt.SavetyOffLine.Util.Debug
{
    [HarmonyPatch(typeof(MainMenu), "HandleEscapeKeypress")]
    public class MainMenu_HandleEscapeKeyPress_Patch_Debug
    {
        public static void Postfix(MainMenu __instance)
        {
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
}