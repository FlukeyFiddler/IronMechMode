using nl.flukeyfiddler.bt.Utils;
using System;

namespace nl.flukeyfiddler.bt.SavingIsForPansies.Util
{
    static class Logger
    {
        private static LogFilePath _logFilePath;
        
        public static void SetLogFilePath(LogFilePath LogFilePath)
        {
            _logFilePath = LogFilePath;
        }

        public static void LogError(Exception ex)
        {
            LoggerUtil.LogError(_logFilePath, ex);
        }

        public static void LogLine(string line)
        {
            LoggerUtil.LogLine(_logFilePath, line);
        }       
    }
}
