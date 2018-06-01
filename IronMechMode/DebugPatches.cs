using BattleTech;
using BattleTech.DataObjects;
using BattleTech.Save;
using BattleTech.Save.SaveGameStructure;
using BattleTech.Serialization;
using BattleTech.Serialization.Handlers;
using BattleTech.Serialization.Models;
using BattleTech.Serialization.Utility;
using BattleTech.UI;
using Harmony;
using nl.flukeyfiddler.bt.IronMechMode.Util;
using System;
using System.Collections.Generic;

namespace nl.flukeyfiddler.bt.IronMechMode
{
    [HarmonyPatch(typeof(MainMenu), "HandleEscapeKeypress")]
    public class MainMenu_HandleEscapeKeyPress_Patch
    {
        public static void Postfix(MainMenu __instance)
        {
            Logger.LogLine("It works");
            //Logger.LogLine(GameInstanceSave.SaveReason.ToString());
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
