﻿using BattleTech.Save.SaveGameStructure;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace nl.flukeyfiddler.bt.IronMechMode.Util
{
    public class ModSettings
    {
        public const SlotGroup CHECKPOINTSAVES_GROUP = SlotGroup.AutoSaves_2;
        public const SlotGroup AUTOSAVES_GROUP = SlotGroup.AutoSaves_1;
        public const SaveReason COMBAT_AUTOSAVE_REASON = SaveReason.COMBAT_GAME_DESIGNER_TRIGGER;

        public static Dictionary<SaveReason, SlotGroup> modAutosaveMapping =
            new Dictionary<SaveReason, SlotGroup>() {
                { SaveReason.COMBAT_GAME_DESIGNER_TRIGGER, AUTOSAVES_GROUP },
                { SaveReason.SIM_GAME_CONTRACT_ACCEPTED, CHECKPOINTSAVES_GROUP},
                { SaveReason.SIM_GAME_COMPLETED_CONTRACT, CHECKPOINTSAVES_GROUP},
            };

        internal static Settings settings = new Settings();

        public static void UpdateSettingsFromJSON(string settingsJSON)
        {
            try
            {
                settings = JsonConvert
                    .DeserializeObject<Settings>(settingsJSON);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                settings = new Settings();
            }
        }

        internal class Settings
        {
            public int MaxManualReloadsPerGame = 2;
            public int MaxAutoSaves = 4;
            public int MaxCheckpointSaves = 2;
        }
    }
}
