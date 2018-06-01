using BattleTech;
using BattleTech.Save.SaveGameStructure;
using Harmony;
using nl.flukeyfiddler.bt.IronMechMode.Util;

namespace nl.flukeyfiddler.bt.IronMechMode
{
    [HarmonyPatch(typeof(GameInstance), "CanSave")]
    public class GameInstance_CanSave_Patch
    {
        static void Postfix(GameInstance __instance, ref bool __result, SaveReason reason)
        {
            if (reason == SaveReason.COMBAT_SIM_STORY_MISSION_RESTART)
            {
                __result = false;
            }
        }
    }

    [HarmonyPatch(typeof(CombatGameState), "TriggerAutoSaving")]
    public class CombatGameState_TriggerAutoSaving_Patch
    {
        static bool Prefix(CombatGameState __instance)
        {
            return false;
        }
    }

    [HarmonyPatch(typeof(TurnDirector), "BeginNewRound")]
    public class TurnDirector_BeginNewRound_Patch
    {
        static void Postfix(TurnDirector __instance)
        {
            Logger.LogLine("TurnDirector_BeginNewRound_Patch");
            __instance.Combat.BattleTechGame.Save(SaveReason.COMBAT_GAME_DESIGNER_TRIGGER, false);
        }
    }

    [HarmonyPatch(typeof(TurnDirector), "EndCurrentRound")]
    public class TurnDirector_EndCurrentRound_Patch
    {
        static void Prefix(TurnDirector __instance)
        {
            Logger.LogLine("TurnDirector_EndCurrentRound_Patch");
            __instance.Combat.BattleTechGame.Save(SaveReason.COMBAT_GAME_DESIGNER_TRIGGER, false);
        }
    }

    [HarmonyPatch(typeof(SlotModel), "_GetDisplayText")]
    public class  SlotModel_GetDisplayText_Patch
    {
        static bool Prefix(SlotModel __instance, ref string __result)
        {
            string debugSaveReason = __instance.SaveReason.ToString();
            __result = "IronMechMode" + "_" + debugSaveReason;
            return false;
        }
    }

    [HarmonyPatch(typeof(SlotGrouping), "set_MaxSaves")]
    public class SlotGrouping_set_MaxSaves_Patch
    {
        public static void Prefix(ref int value)
        {
            value = 4;
        }
    }
}