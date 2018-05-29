using Harmony;
using nl.flukeyfiddler.bt.Utils;
using System.Reflection;

namespace nl.flukeyfiddler.bt.SavingIsForPansies
{
    public class SavingIsForPansies
    {

        public static void Init(string modDirectory, string settingsJSON)
        {
            var harmony = HarmonyInstance.Create("nl.flukeyfiddler.bt.SavingIsForPansies");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
            Logger.LogLine("mod dir: " + modDirectory);
            Logger.LogLine("json settings: " + settingsJSON);

        }
    }
}
