using Harmony;
using System.Reflection;

namespace nl.flukeyfiddler.SavingIsForPuppies
{
    public class SavingIsForPuppies
    {
        public static void Init()
        {
            var harmony = HarmonyInstance.Create("nl.flukeyfiddler.SavingIsForPuppies");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }
    }
}
