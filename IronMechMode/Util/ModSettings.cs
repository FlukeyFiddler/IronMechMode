using BattleTech.Save.SaveGameStructure;
using Newtonsoft.Json;
using System;

namespace nl.flukeyfiddler.bt.IronMechMode.Util
{
    public class ModSettings
    {
        public const SaveReason COMBAT_SAVE_REASON = SaveReason.COMBAT_GAME_DESIGNER_TRIGGER;

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
                Logger.LogError(ex);
                settings = new Settings();
            }
        }

        internal class Settings
        {
            public int MaxManualReloadsPerGame = 2;
            // MAX_SIM_GAME_SAVES
            // MAX_COMBAT_GAME_SAVES
        }
    }
}
