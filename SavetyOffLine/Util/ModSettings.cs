using BattleTech.Save.SaveGameStructure;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace nl.flukeyfiddler.bt.SavetyOffLine.Util
{
    public class ModSettings
    {
        public const SlotGroup SIMGAME_SAVES_GROUP = SlotGroup.AutoSaves_2;
        public const SlotGroup COMBATGAME_SAVES_GROUP = SlotGroup.AutoSaves_1;
        public const SaveReason COMBATGAME_AUTOSAVE_REASON = SaveReason.COMBAT_GAME_DESIGNER_TRIGGER;
        public const SaveReason SIMGAME_AUTOSAVE_REASON = SaveReason.SIM_GAME_EVENT_FIRED;
        public const string MOD_SAVE_REFERENCECONTAINER_KEY = "SOLAcceptedContract";

        public static Dictionary<SaveReason, SlotGroup> AutosaveMapping =
            new Dictionary<SaveReason, SlotGroup>() {
                { SaveReason.COMBAT_GAME_DESIGNER_TRIGGER, COMBATGAME_SAVES_GROUP },
                { SaveReason.COMBAT_SIM_STORY_MISSION_RESTART, COMBATGAME_SAVES_GROUP },
                { SaveReason.SIM_GAME_CONTRACT_ACCEPTED, SIMGAME_SAVES_GROUP },
                { SaveReason.SIM_GAME_COMPLETED_CONTRACT, SIMGAME_SAVES_GROUP },
                { SaveReason.SIM_GAME_FIRST_SAVE, SIMGAME_SAVES_GROUP },
                { SaveReason.SIM_GAME_QUARTERLY_REPORT, SIMGAME_SAVES_GROUP },
                { SaveReason.SIM_GAME_EVENT_FIRED, SIMGAME_SAVES_GROUP },
                { SaveReason.SIM_GAME_EVENT_OPTION_SELECTED, SIMGAME_SAVES_GROUP },
                { SaveReason.SIM_GAME_EVENT_RESOLVED, SIMGAME_SAVES_GROUP },
                { SaveReason.SIM_GAME_ARRIVED_AT_PLANET, SIMGAME_SAVES_GROUP },
                { SaveReason.SIM_GAME_BREADCRUMB_COMPLETE, SIMGAME_SAVES_GROUP },
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
            public int CombatGameAutoSaves = 4;
            public int SimGameAutoSaves = 4;
        }
    }
}
