using BattleTech;
using BattleTech.Save.SaveGameStructure;
using Harmony;
using nl.flukeyfiddler.bt.IronMechMode.Util;
using nl.flukeyfiddler.bt.IronMechMode.Util.Debug;
using System;

namespace nl.flukeyfiddler.bt.IronMechMode
{
    public class Helper
    {
        public static void TriggerSimGameAutosave()
        {
            UnityGameInstance.BattleTechGame.Simulation.TriggerSaveNow(ModSettings.SIMGAME_AUTOSAVE_REASON, true);
        }
    }
}