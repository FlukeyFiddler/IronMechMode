using Harmony;
using nl.flukeyfiddler.bt.IronMechMode.Util;
using nl.flukeyfiddler.bt.Utils.Logger;
using System.IO;
using System.Reflection;

namespace nl.flukeyfiddler.bt.IronMechMode
{
    public class IronMechMode
    {
        public static void Init(string modDirectory, string settingsJSON)
        {
            var harmony = HarmonyInstance.Create("nl.flukeyfiddler.bt.IronMechMode");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
            
            Logger.SetLogFilePath(new LogFilePath(Path.Combine(modDirectory, "Log.txt")));
            Logger.GameStarted();

            ModSettings.UpdateSettingsFromJSON(settingsJSON);
        }
    }
}
