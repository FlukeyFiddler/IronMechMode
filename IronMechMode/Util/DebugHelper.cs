using BattleTech;
using BattleTech.Save.SaveGameStructure;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace nl.flukeyfiddler.bt.IronMechMode.Util.Debug
{
    public static class DebugHelper
    {
        public static void LogCombatGameStateCanSave(CombatGameState __instance)
        {
            bool flag = __instance.LocalPlayerTeam == __instance.TurnDirector.ActiveTurnActor;
            bool flag2 = __instance.StackManager.CanSave(false);
            bool flag3 = false;
            bool isTutorial = __instance.ActiveContract.IsTutorial;

            if (flag)
            {
                foreach (AbstractActor abstractActor in __instance.LocalPlayerTeam.units)
                {
                    if (abstractActor.HasBegunActivation && !abstractActor.HasActivatedThisRound)
                    {
                        flag3 = true;
                        break;
                    }
                }
            }

            string[] lines = new string[]
            {
                "flag (is this the active team?): " + flag,
                "flag2 (what's the bool in the stackManager?): " + flag2,
                "!flag3 (has any unit in the Local Player team begun activivation" +
                    " and has it not activated this round?): " + !flag3,
                "!isTutorial: " + !isTutorial,
                "sum of flag2 && flag && !flag3 && !isTutorial: " + (flag2 && flag && !flag3 && !isTutorial)
            };
            Logger.Block(lines, MethodBase.GetCurrentMethod());
        }

       
        public static void LogGameInstanceCanSave(GameInstance __instance, SaveReason reason)
        {
            List<string> debugLines = new List<string>();
            
            if((__instance.Combat == null && __instance.Simulation == null) || __instance.IsMultiplayer)
            {
                debugLines.Add("Return FALSE, No instance of: ");
                debugLines.Add("combat: " + (__instance.Combat == null));
                debugLines.Add("simgame: " + (__instance.Simulation == null));
                debugLines.Add("or is multiplayer: " + __instance.IsMultiplayer);
            }

            if (__instance.SaveManager.GameInstanceSaves.Saving)
            {
                debugLines.Add("Return FALSE, is already Saving");
            }

            if (__instance.Combat != null)
            {
                debugLines.Add("CombatGameState.CanSave returns: " + __instance.Combat.CanSave(false));
            }

            debugLines.Add("SimGameState.CanSave returns: " + __instance.Simulation.CanSave(reason, false));

            Logger.Block(debugLines.ToArray(), MethodBase.GetCurrentMethod());
        }
        
    }
}
