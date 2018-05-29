using System;
using System.IO;


namespace nl.flukeyfiddler.bt.SavingIsForPansies
{
    public class Logger
    {
        private readonly string logFilePath;

        Logger(string modDirPath)
        {
            logFilePath = Path.Combine(modDirPath, "Log.txt");
        }
        
        public void LogError(Exception ex)
        {
            Utils.Logger.LogError(logFilePath, ex);
        }

        public void LogLine(String line)
        {
            Utils.Logger.LogLine(logFilePath, line);
        }
    }
}
