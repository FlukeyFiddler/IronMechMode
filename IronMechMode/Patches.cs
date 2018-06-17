using BattleTech;
using BattleTech.Framework;
using BattleTech.Save;
using BattleTech.Save.SaveGameStructure;
using BattleTech.UI;
using Harmony;
using nl.flukeyfiddler.bt.IronMechMode.Util;
using nl.flukeyfiddler.bt.IronMechMode.Util.Debug;
using nl.flukeyfiddler.bt.Utils;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace nl.flukeyfiddler.bt.IronMechMode
{
    [HarmonyPatch(typeof(GameInstance), "CanSave")]
    public class GameInstance_CanSave_Patch
    {
        static void Postfix(GameInstance __instance, ref bool __result, SaveReason reason)
        {
            if (reason == SaveReason.MANUAL)
            {
                __result = false;
            }
        }
    }

    [HarmonyPatch(typeof(TurnDirector), "IncrementActiveTurnActor")]
    public static class TurnDirector_IncrementActiveTurnActor_Patch
    {
        public static int lastRound = 0;

        private static void Postfix(TurnDirector __instance)
        {
            bool playerTeamTurn = __instance.ActiveTurnActor.GUID == __instance.Combat.LocalPlayerTeamGuid;

            if (playerTeamTurn && __instance.CurrentRound > lastRound)
            {
                lastRound = __instance.CurrentRound;

                __instance.Combat.BattleTechGame.Save(ModSettings.COMBATGAME_AUTOSAVE_REASON, false);
            }
        }
    }

    [HarmonyPatch(typeof(SlotModel), "_GetDisplayText")]
    public class SlotModel_GetDisplayText_Patch
    {
        static bool Prefix(SlotModel __instance, ref string __result)
        {
            string debugSaveReason = __instance.SaveReason.ToString();
            __result = __instance.SaveReason.ToString();
            return false;
        }
    }

    [HarmonyPatch(typeof(ReasonToSlotGroupMapping), "GetSlotGroup")]
    public class ReasonToSlotGroupMapping_GetSlotGroup_Patch
    {
        static void Prefix(SaveReason reason, ref Dictionary<SaveReason, SlotGroup> ___mapping)
        {
            foreach (KeyValuePair<SaveReason, SlotGroup> _override in ModSettings.AutosaveMapping)
            {
                ___mapping[_override.Key] = _override.Value;
            }
        }
    }

    [HarmonyPatch(typeof(SlotGroupingParametersMapping), "GetSlotGroupings")]
    public class SlotGroupingParametersMapping_Patch
    {
        public static void Postfix(ref Dictionary<SlotGroup, SlotGrouping> __result)
        {
            __result[ModSettings.AUTOSAVES_GROUP] = new SlotGrouping(ModSettings.settings.MaxAutoSaves, SlotGroupFullBehavior.AUTO_OVERWRITE_OLDEST);
            __result[ModSettings.CHECKPOINTSAVES_GROUP] = new SlotGrouping(ModSettings.settings.MaxCheckpointSaves, SlotGroupFullBehavior.AUTO_OVERWRITE_OLDEST);
        }
    }

    [HarmonyPatch(typeof(CombatGameState), "OnCombatGameDestroyed")]
    public class CombatGameState_OnCombatGameDestroyed_Patch
    {
        public static void Postfix() {
            TurnDirector_IncrementActiveTurnActor_Patch.lastRound = 0;
        }
    }


    [HarmonyPatch(typeof(GameInstance), "Save")]
    public class GameInstance_Save_Patch
    {
        public static void Prefix(GameInstance __instance, SaveReason reason, out Contract __state)
        {
            if (reason != SaveReason.SIM_GAME_CONTRACT_ACCEPTED) {
                __state = null;
                return;

            }
             __state = __instance.Simulation.SelectedContract;
        }

        public static void Postfix(ref GameInstance __instance, SaveReason reason, Contract __state)
        {
            if (reason != SaveReason.SIM_GAME_CONTRACT_ACCEPTED)
                return;

            SimGameState simGame = __instance.Simulation;
            Contract selectedContract = __state;
            Logger.Minimal("selected Contract: " + selectedContract.Name);
            List<ContractData> contractData = new List<ContractData>();

            foreach (ContractData contract in Traverse.Create(simGame).Field("contractBits").GetValue<List<ContractData>>())
            {
                Logger.Minimal("got me a contractBit");
                Logger.Minimal("contract name: " + contract.conName);
                if (selectedContract.Name == contract.conName)
                {
                    Logger.Minimal("found contract: " + contract.conName);
                    contractData.Add(contract);
                }
            }

            Traverse.Create(simGame).Field("globalContracts").SetValue(new List<Contract> { selectedContract});
            Traverse.Create(simGame).Field("contractBits").SetValue(contractData);
        }
    }
}