using Harmony;
using System.IO;
using System.Reflection;

namespace nl.flukeyfiddler.bt.SavingIsForPansies
{
    public class SavingIsForPuppies
    {

        public static void Init()
        {
            var harmony = HarmonyInstance.Create("nl.flukeyfiddler.bt.SavingIsForPansies");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
            

        }
    }
}
