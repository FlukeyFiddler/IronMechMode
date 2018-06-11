using BattleTech;
using BattleTech.Save.SaveGameStructure;
using Newtonsoft.Json;
using nl.flukeyfiddler.bt.Utils;
using System;
using System.Collections.Generic;

namespace nl.flukeyfiddler.bt.IronMechMode.Util
{
    public class ModSettings
    {
        public const SlotGroup CHECKPOINTSAVES_GROUP = SlotGroup.AutoSaves_2;
        public const SlotGroup AUTOSAVES_GROUP = SlotGroup.AutoSaves_1;
        public const SaveReason COMBATGAME_AUTOSAVE_REASON = SaveReason.COMBAT_GAME_DESIGNER_TRIGGER;
        public const SaveReason SIMGAME_AUTOSAVE_REASON = SaveReason.SIM_GAME_EVENT_FIRED;

        public static Dictionary<SaveReason, SlotGroup> AutosaveMapping =
            new Dictionary<SaveReason, SlotGroup>() {
                { SaveReason.COMBAT_GAME_DESIGNER_TRIGGER, AUTOSAVES_GROUP },
                { SaveReason.SIM_GAME_CONTRACT_ACCEPTED, CHECKPOINTSAVES_GROUP },
                { SaveReason.SIM_GAME_COMPLETED_CONTRACT, CHECKPOINTSAVES_GROUP },
            };

        private static Dictionary<object, Dictionary<MethodName, ShouldSaveCondition>> autosavePatches;

        internal static Settings settings = new Settings();

        public static Dictionary<object, Dictionary<MethodName, ShouldSaveCondition>> AutosavePatches { get => autosavePatches; set => autosavePatches = value; }

        public static void UpdateSettingsFromJSON(string settingsJSON)
        {
            try
            {
                settings = JsonConvert
                    .DeserializeObject<Settings>(settingsJSON);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                settings = new Settings();
            }
        }

        public static void InstantiateAutoSavePatches(SimGameState simGame) {
            AutosavePatches = new Dictionary<object, Dictionary<MethodName, ShouldSaveCondition>>()
            {
                {simGame,
                    new Dictionary<MethodName, ShouldSaveCondition>() {
                            { new MethodName("AddArgoUpgrade"), new DefaultShouldSaveConditon() },
                            { new MethodName("CompleteBreadcrumb"), new DefaultShouldSaveConditon() },
                            { new MethodName("PruneWorkOrder"), new PruneWorkOrderSaveConditon() },
                    }
                },
            };
        }


        internal class Settings
        {
            public int MaxManualReloadsPerGame = 2;
            public int MaxAutoSaves = 4;
            public int MaxCheckpointSaves = 2;
        }
    }
}
