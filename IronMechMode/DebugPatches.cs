using BattleTech.UI;
using Harmony;
using nl.flukeyfiddler.bt.IronMechMode.Util;

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
}



/*
 * BattleTech.Save.SaveGameStructure.Messages
 * BattleTech.CombatGameState.CanSave
 * BattleTech.SimGameState.CanSave
 */
