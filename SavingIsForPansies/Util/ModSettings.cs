using Newtonsoft.Json;
using System;

namespace nl.flukeyfiddler.bt.SavingIsForPansies.Util
{
    public class ModSettings
    {
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
        }
    }
}
