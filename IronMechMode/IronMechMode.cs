using Harmony;
using nl.flukeyfiddler.bt.IronMechMode.Util;
using nl.flukeyfiddler.bt.Utils.Logger;
using System.IO;
using System.Reflection;

namespace nl.flukeyfiddler.bt.IronMechMode
{
    public class IronMechMode
    {
        public static HarmonyInstance harmony; 

        public static void Init(string modDirectory, string settingsJSON)
        {
            harmony = HarmonyInstance.Create("nl.flukeyfiddler.bt.IronMechMode");
            ModSettings.UpdateSettingsFromJSON(settingsJSON);

            Logger.SetLogFilePath(new LogFilePath(Path.Combine(modDirectory, "Log.txt")));
            Logger.GameStarted();
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }
    }
}
