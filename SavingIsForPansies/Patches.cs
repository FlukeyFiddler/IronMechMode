using BattleTech.UI;
using Harmony;
using nl.flukeyfiddler.bt.SavingIsForPansies.Util;

namespace nl.flukeyfiddler.bt.SavingIsForPuppies
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
