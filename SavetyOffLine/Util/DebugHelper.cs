using BattleTech;
using BattleTech.Save.SaveGameStructure;
using Harmony;
using System.Collections.Generic;
using System.Reflection;

namespace nl.flukeyfiddler.bt.SavetyOffLine.Util.Debug
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

            if (flag2 && flag && !flag3 && !isTutorial) { return; }
                
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

            
            if (flag2 == false && !isTutorial)
            {
                Logger.Minimal("flag 2 (stackmanager) is false");
                LogStackManagerCanSave(__instance.StackManager);
            }
        }

        internal static void MessageCenterMessageReceived(MessageCenterMessage message)
        {
            Logger.Line("Message received of type: " + message.MessageType + ", autosave should be triggered");
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

        public static void LogStackManagerCanSave(StackManager stackmanager)
        {
            List<IStackSequence> SequenceStack = Traverse.Create(stackmanager).Property("SequenceStack").GetValue<List<IStackSequence>>();
            List<IStackSequence> ParallelStack = Traverse.Create(stackmanager).Property("ParallelStack").GetValue<List<IStackSequence>>();

            List<string> debugLines = new List<string>();
            debugLines.Add("stackManagerSequenceStackCount (should be 2): " + SequenceStack.Count);
            
            if(SequenceStack != null)
            {
                foreach(IStackSequence sequence in SequenceStack)
                {
                    debugLines.Add("SequenceStack messageIndex: " + sequence.MessageIndex);
                    debugLines.Add("Desired Parent Name" + sequence.DesiredParentType.FullName);
                }
            }
            else
            {
                Logger.Minimal("SequensceStack is null!");
            }

            if(ParallelStack != null)
            {
                foreach (IStackSequence sequence in ParallelStack)
                {
                    debugLines.Add("ParallelStack messageIndex: " + sequence.MessageIndex);
                    debugLines.Add("Desired Parent Name" + sequence.DesiredParentType.FullName);
                }
            }
            else
            {
                Logger.Minimal("ParallelStack is null!");
            }

            Logger.Minimal("about to spit it out from LogStackManagerCanSave");
            
            Logger.Block(debugLines.ToArray(), MethodBase.GetCurrentMethod());
        }
    }
}
