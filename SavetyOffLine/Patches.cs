using BattleTech;
using BattleTech.Save.SaveGameStructure;
using BattleTech.UI;
using Harmony;
using nl.flukeyfiddler.bt.SavetyOffLine.Util;
using System;
using System.Collections.Generic;

namespace nl.flukeyfiddler.bt.SavetyOffLine
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
        static void Postfix(SlotModel __instance, ref string __result)
        {
            __result = "S.O.L. " ;

            SaveReason saveReason = __instance.SaveReason;
            var autosaveMapping = ModSettings.AutosaveMapping;

            if (autosaveMapping.ContainsKey(saveReason))
            {
                __result += (autosaveMapping[saveReason] == ModSettings.COMBATGAME_SAVES_GROUP) ?
                    "Combat Autosave" : "Sim Autosave";
            }
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
            __result[ModSettings.COMBATGAME_SAVES_GROUP] = new SlotGrouping(ModSettings.settings.CombatGameAutoSaves, SlotGroupFullBehavior.AUTO_OVERWRITE_OLDEST);
            __result[ModSettings.SIMGAME_SAVES_GROUP] = new SlotGrouping(ModSettings.settings.SimGameAutoSaves, SlotGroupFullBehavior.AUTO_OVERWRITE_OLDEST);
        }
    }

    [HarmonyPatch(typeof(CombatGameState), "OnCombatGameDestroyed")]
    public class CombatGameState_OnCombatGameDestroyed_Patch
    {
        public static void Postfix() {
            TurnDirector_IncrementActiveTurnActor_Patch.lastRound = 0;
        }
    }

    [HarmonyPatch(typeof(InstanceModel), "GetAllSlots")]
    public class InstanceModel_GetAllSlots_Patch
    {
        public static void Postfix(ref List<SlotModel> __result)
        {
            __result.RemoveRange(1, __result.Count - 1);
        }
    }

    [HarmonyPatch(typeof(SGSaveGameListViewItem), "SetData")]
    public class SGSaveGameListViewItem_SetData_Patch
    {
        public static void Postfix(SGSaveGameListViewItem __instance)
        {
            Traverse.Create(__instance).Field("deleteButton").GetValue<HBSButton>().
                SetState(ButtonState.Disabled);
        }
    }


    [HarmonyPatch(typeof(SGSaveGameListView), "OnRowClicked")]
    public class SGSaveGameListView_OnRowClicked_Patch
    {
        protected static void Prefix(SGSaveGameListView __instance, ref SGSaveGameListViewItem row)
        {
            row.SetState(ButtonState.Disabled);
        }
    }

    [HarmonyPatch(typeof(SGSaveGameSlotsPanel), "OnSlotSelected")]
    public class SGSaveGameSlotsPanel_OnSlotSelected_Patch
    {
        public static void Postfix(ref SGSaveGameSlotsPanel __instance, SlotModel slot)
        {
            if (slot.IsSkirmish)
                return;

            __instance.DisableLoadButton();
        }
    }

    [HarmonyPatch(typeof(SGLoadSavedGameScreen), "HandleEnterKeypress")]
    public class SGLoadSavedGameScreen_HandleEnterKeypress_Patch
    {
        public static bool Prefix()
        {
            return false;
        }
    }

    [HarmonyPatch(typeof(SimGameOptionsMenu), "QuitPopup")]
    public class SimGameOptionsMenu_QuitPopup_Patch
    {
        private static bool Prefix(SimGameOptionsMenu __instance, Action quitAction)
        {
            if (UnityGameInstance.BattleTechGame.IsMultiplayer)
                return true;

            MessageCenter messageCenter = Traverse.Create(__instance).Field("messageCenter").GetValue<MessageCenter>();

            GenericPopupBuilder.
                Create("Are you sure you want to quit?", String.Empty).
                AddButton("Cancel", null, true, null).
                AddButton("Save & Quit", delegate
                    {
                        messageCenter.AddFiniteSubscriber(MessageCenterMessageType.BlockSaved, delegate (MessageCenterMessage message)
                        {
                            quitAction();
                            return true;
                        });
                        UnityGameInstance.BattleTechGame.Save(ModSettings.SIMGAME_AUTOSAVE_REASON, false);
                    }, true, null).
                CancelOnEscape().IsNestedPopupWithBuiltInFader().Render();
            return false;
        }
    }
}