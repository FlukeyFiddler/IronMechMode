using BattleTech;
using BattleTech.Save.SaveGameStructure;
using BattleTech.UI;
using Harmony;
using System.Collections.Generic;

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
                DebugHelper.LogGameInstanceCanSave(__instance, reason);
            }
        }
    }
    
    [HarmonyPatch(typeof(CombatGameState), "CanSave")]
    public class CombatGameState_Save_Patch_Debug
    {
        static void Postfix(CombatGameState __instance, bool __result)
        {
            if(!__result)
            {
                DebugHelper.LogCombatGameStateCanSave(__instance);
            }
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
