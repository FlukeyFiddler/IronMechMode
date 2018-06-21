using BattleTech;
using Harmony;
using System.Collections.Generic;
using System.Reflection;

namespace nl.flukeyfiddler.bt.SavetyOffLine.Util
{
    public static class EnableOrDisablePatchesHelper
    {
        private static HarmonyInstance harmony = SavetyOffLine.harmony;

       

        public static bool patchesPreviouslyDisabled = false;


        public static void EnableOrDisablePatches(SimGameState simGame)
        {
            var ironManCampaignField = Traverse.Create(simGame).Property("IsIronmanCampaign");

            if (!ironManCampaignField.FieldExists())
                return;

            bool isIronmanCampaign = ironManCampaignField.GetValue<bool>();

            if (isIronmanCampaign)
            {
                if (!patchesPreviouslyDisabled)
                {
                    disablePatches();
                }
            }
            else if (patchesPreviouslyDisabled)
            {
                enablePatches();
            }
        }

        private static void enablePatches()
        {
            Logger.Line("Enabling SavetyOffLine after an IronManCampaign was played", MethodBase.GetCurrentMethod());

            patchesPreviouslyDisabled = false;

            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }

        private static void disablePatches()
        {
            Logger.Line("Disabling SavetyOffLine as this campaign uses IronManMode", MethodBase.GetCurrentMethod());

            patchesPreviouslyDisabled = true;

            List<MethodBase> methodsToRemove = new List<MethodBase>();

            foreach (MethodBase patchedMethod in harmony.GetPatchedMethods()) {
                if (ModSettings.patchesToKeepIfIronmanCampaign.Contains(patchedMethod))
                {
                    Logger.Minimal("not removing: " +patchedMethod.Name);
                    continue;
                }

                addMethodToRemoveListIfPatchedByMe(patchedMethod, methodsToRemove);
            }

            methodsToRemove.Do(delegate (MethodBase methodToRemove) {
                SavetyOffLine.harmony.RemovePatch(methodToRemove, HarmonyPatchType.All, SavetyOffLine.harmony.Id);
            });
        }

        private static void addMethodToRemoveListIfPatchedByMe(MethodBase patchedMethod, List<MethodBase> methodsToRemove)
        {
            harmony.GetPatchInfo(patchedMethod).Owners.DoIf(isPatchedByMe, delegate {
                methodsToRemove.Add(patchedMethod);
            });
        }

        private static bool isPatchedByMe(string owner)
        {
            return owner == harmony.Id;
        }
    }
}
