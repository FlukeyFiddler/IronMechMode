using Harmony;
using Newtonsoft.Json;
using nl.flukeyfiddler.bt.SavingIsForPansies.Util;
using nl.flukeyfiddler.bt.Utils;
using System;
using System.IO;
using System.Reflection;

namespace nl.flukeyfiddler.bt.SavingIsForPansies
{
    public class SavingIsForPansies
    {
       

        public static void Init(string modDirectory, string settingsJSON)
        {
            var harmony = HarmonyInstance.Create("nl.flukeyfiddler.bt.SavingIsForPansies");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
            
            Logger.SetLogFilePath(new LogFilePath(Path.Combine(modDirectory, "Log.txt")));
            ModSettings.UpdateSettingsFromJSON(settingsJSON);
        }
    }
}
