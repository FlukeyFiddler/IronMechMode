using Harmony;
using Newtonsoft.Json;
using System;
using System.Reflection;

namespace nl.flukeyfiddler.bt.SavingIsForPansies
{
    public class SavingIsForPansies
    {
        internal static ModSettings Settings = new ModSettings();

        public static void Init(string modDirectory, string settingsJSON)
        {
            var harmony = HarmonyInstance.Create("nl.flukeyfiddler.bt.SavingIsForPansies");
            harmony.PatchAll(Assembly.GetExecutingAssembly());

            ParseSettingsFromJSON(settingsJSON);
           
        }

        private static void ParseSettingsFromJSON(string settingsJSON) {
            try
            {
                Settings = JsonConvert
                    .DeserializeObject<ModSettings>(settingsJSON);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                Settings = new ModSettings();
            }
        }

        internal class ModSettings
        {
            public int MaxReloadsPerGame = 2;
        }
    }
}
