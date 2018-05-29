using BattleTech.UI;
using Harmony;

namespace nl.flukeyfiddler.SavingIsForPuppies
{
    [HarmonyPatch(typeof(MainMenu), "HandleEscapeKeypress")]
    public class MainMenu_HandleEscapeKeyPress_Patch
    {
        public static void Postfix(MainMenu __instance)
        {
        }
    }
}
