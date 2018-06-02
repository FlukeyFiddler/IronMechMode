using BattleTech;
using BattleTech.Save.SaveGameStructure;
using BattleTech.UI;
using Harmony;
using nl.flukeyfiddler.bt.IronMechMode.Util;
using System.Reflection;

namespace nl.flukeyfiddler.bt.IronMechMode
{
    [HarmonyPatch(typeof(MainMenu), "HandleEscapeKeypress")]
    public class MainMenu_HandleEscapeKeyPress_Patch
    {
        public static void Postfix(MainMenu __instance)
        {
            Logger.LogLine("It works");
        }
    }

    [HarmonyPatch(typeof(GameInstance), "Save")]
    public class GameInstance_Save_Patch
    {
        static void Prefix(GameInstance __instance, SaveReason reason)
        {
            MethodBase curr = MethodBase.GetCurrentMethod();
            string[] debugLines = new string[]
            {
                "Trying to save",
                "saveReason: " + reason,
                "canSave: " + __instance.CanSave(reason, false),
            };
            Logger.LogBlock(debugLines, MethodBase.GetCurrentMethod());
            Logger.LogBlock(debugLines);
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
