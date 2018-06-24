using BattleTech;
using BattleTech.Save;
using BattleTech.Save.Test;
using Harmony;
using nl.flukeyfiddler.bt.SavetyOffLine.Util;
using System.Collections.Generic;

namespace nl.flukeyfiddler.bt.SavetyOffLine
{

    [HarmonyPatch(typeof(SimGameState), "_OnAttachUXComplete")]
    public class SimGameState_OnAttachUXComplete_Patch
    {
        private static void Prefix(SimGameState __instance, out bool __state)
        {
            __state = __instance.IsFromSave;
        }

        private static void Postfix(SimGameState __instance, bool __state)
        {
            if (__state && __instance.SelectedContract != null)
            {
                __instance.ForceTakeContract(__instance.SelectedContract, false);
            }
        }
    }


    [HarmonyPatch(typeof(SimGameState), "Dehydrate")]
    public class SimGameState_Dehydrate_Patch
    {
        public static void Prefix(SimGameState __instance, SimGameSave save, ref SerializableReferenceContainer references)
        {
            Contract selectedContract = __instance.SelectedContract;

            if (selectedContract == null || __instance.BattleTechGame.Combat != null)
                return;
          
            references.AddItem(ModSettings.MOD_SAVE_REFERENCECONTAINER_KEY, selectedContract);
        }
    }
       
    [HarmonyPatch(typeof(SimGameState), "Rehydrate")]
    public class SimGameState_Rehydrate_Patch
    {
        public static void Postfix(SimGameState __instance, GameInstanceSave gameInstanceSave,
           ref List<Contract> ___globalContracts)
        {

            SimGameSave save = gameInstanceSave.SimGameSave;

            if (!save.GlobalReferences.HasItem(ModSettings.MOD_SAVE_REFERENCECONTAINER_KEY))
                return;

            Contract selectedContract = save.GlobalReferences.GetItem<Contract>(ModSettings.MOD_SAVE_REFERENCECONTAINER_KEY);

            if (selectedContract == null)
                return;

            //LanceConfiguration lastLance = Traverse.Create(__instance).Method("GetLastLance").GetValue<LanceConfiguration>();
            //selectedContract.SetLanceConfiguration(lastLance);
            //selectedContract.Override.disableLanceConfiguration = false;

            selectedContract.Override.disableNegotations = true;
            selectedContract.Override.disableCancelButton = true;
            selectedContract.Override.negotiatedSalary = selectedContract.PercentageContractValue;
            selectedContract.Override.negotiatedSalvage = selectedContract.PercentageContractSalvage;

            __instance.SetSelectedContract(selectedContract);
        }
    }    
}
