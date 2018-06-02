using Harmony;
using nl.flukeyfiddler.bt.Utils;
using System;
using System.Reflection;

namespace nl.flukeyfiddler.bt.IronMechMode.Util
{
    static class Logger
    {
        private static LogFilePath logFilePath;
        
        public static void SetLogFilePath(LogFilePath logFilePath)
        {
            Logger.logFilePath = logFilePath;
        }

        public static void LogError(Exception ex, MethodBase caller = null)
        {
            LoggerUtil.LogError(logFilePath, ex, caller);
        }

        public static void LogLine(string line, MethodBase caller = null)
        {
            LoggerUtil.LogLine(logFilePath, line, caller);
        }

        public static void LogMinimal(string line)
        {
            LoggerUtil.LogMinimal(logFilePath, line);
        }

        public static void LogBlock(string[] lines, MethodBase caller = null)
        {
            LoggerUtil.LogBlock(logFilePath, lines, caller);
        }
    }
}
