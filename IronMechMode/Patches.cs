using BattleTech;
using BattleTech.UI;
using Harmony;
using nl.flukeyfiddler.bt.IronMechMode.Util;

namespace nl.flukeyfiddler.bt.IronMechMode
{
    [HarmonyPatch(typeof(GameInstance), "CanSave")]
    public class GameInstance_CanSave_Patch
    {
        public static void Postfix(GameInstance __instance, ref bool __result)
        {
            Logger.LogLine("in postfix");
            //__result = false;
        }
    }
}
