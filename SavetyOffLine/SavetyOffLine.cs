using Harmony;
using nl.flukeyfiddler.bt.SavetyOffLine.Util;
using nl.flukeyfiddler.bt.Utils.Logger;
using System.IO;
using System.Reflection;

namespace nl.flukeyfiddler.bt.SavetyOffLine
{
    public class SavetyOffLine
    {
        public static HarmonyInstance harmony; 

        public static void Init(string modDirectory, string settingsJSON)
        {
            harmony = HarmonyInstance.Create("nl.flukeyfiddler.bt.SavetyOffLine");
            ModSettings.UpdateSettingsFromJSON(settingsJSON);

            Logger.SetLogFilePath(new LogFilePath(Path.Combine(modDirectory, "Log.txt")));
            Logger.GameStarted();
            harmony.PatchAll(Assembly.GetExecutingAssembly());

        }
    }
}
