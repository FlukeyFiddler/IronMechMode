using BattleTech;
using BattleTech.Save.SaveGameStructure;
using BattleTech.UI;
using Harmony;
using nl.flukeyfiddler.bt.IronMechMode.Util;
using nl.flukeyfiddler.bt.IronMechMode.Util.Debug;
using System;
using System.Reflection;

namespace nl.flukeyfiddler.bt.IronMechMode
{
    [HarmonyPatch(typeof(GameInstance), "CanSave")]
    public class GameInstance_CanSave_Patch
    {
        // This thing gets called 40*/s, maybe want to overide something lower
        static void Postfix(GameInstance __instance, ref bool __result, SaveReason reason)
        {
            if (reason == SaveReason.MANUAL)
            {
                //__result = false;
            }           
        }
    }
    
    [HarmonyPatch(typeof(CombatGameState), "_Init")]
    public class CombatGameState_Init_Patch
    {
        private static CombatGameState combatGameState;

        private static void Postfix(CombatGameState __instance)
        {
            combatGameState = __instance;

            Logger.Line("TODO: test that this subscriber does not need to be destroyed on " +
                "OnCombatGameDestroyed", MethodBase.GetCurrentMethod());

            // Alas MessageCenterMessageType.OnRoundBeginComplete doesn't seem to be implemented
            // Grr neither OnPhaseBeginComplete
            __instance.MessageCenter.AddSubscriber(MessageCenterMessageType.OnTurnActorActivate,
                new ReceiveMessageCenterMessage(OnTurnActorActivateMessage));
        }
        //  new ReceiveMessageCenterMessage(OnTurnActorActivateMessage)
        //  PhaseBeginCompleteMessage(int round, int phase, string TurnActorGUID, int available)
        public static void OnTurnActorActivateMessage(MessageCenterMessage message)
        {
            TurnActorActivateMessage turnActorActivateMessage = message as TurnActorActivateMessage;

            if (turnActorActivateMessage.TurnActorGUID == combatGameState.LocalPlayerTeamGuid)
            {
                //combatGameState.BattleTechGame.Save(SaveReason.COMBAT_GAME_DESIGNER_TRIGGER, false);
              //  UnityGameInstance.BattleTechGame.Save(SaveReason.COMBAT_GAME_DESIGNER_TRIGGER, false);
            }
        }
    }
    

    /*
    [HarmonyPatch(typeof(TurnEventNotification), "ShowPlayerTeamNotify")]
    public class TurnEventNotification_ShowPlayerTeamNotify_Patch
    {
        private static void Postfix(CombatGameState ___Combat)
        {
            //___Combat.BattleTechGame.Save(SaveReason.COMBAT_GAME_DESIGNER_TRIGGER, false);
        }
    }
    */

    [HarmonyPatch(typeof(TurnDirector), "EndCurrentRound")]
    public class TurnDirector_EndCurrentRound_Patch
    {
        static void Postfix(TurnDirector __instance)
        {
            //if (__instance.Combat.CanSave(false))
            //{
                // Hmm this doesn't save in combat but does after it
           // __instance.Combat.BattleTechGame.Save(SaveReason.COMBAT_GAME_DESIGNER_TRIGGER, false);
            //}

        }
    }
    
    [HarmonyPatch(typeof(SlotModel), "_GetDisplayText")]
    public class  SlotModel_GetDisplayText_Patch
    {
        static void Postfix(SlotModel __instance, ref string __result)
        {
            string debugSaveReason = __instance.SaveReason.ToString();
            //__result = "IronMechMode" + "_" + debugSaveReason;
            //return false;
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