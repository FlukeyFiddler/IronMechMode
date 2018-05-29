using System;
using System.IO;

namespace nl.flukeyfiddler.bt.SavingIsForPansies
{
    public static class Logger
    {
        const string LOG_FILE_PATH = "mods\\SavingIsForPansies\\log.txt";

        public static void LogError(Exception ex)
        {
            using (StreamWriter writer = new StreamWriter(LOG_FILE_PATH, true))
            {
                writer.WriteLine("Message :" + ex.Message + "<br/>" + Environment.NewLine + "StackTrace :" + ex.StackTrace +
                   "" + Environment.NewLine + "Date :" + DateTime.Now.ToString());
                writer.WriteLine(Environment.NewLine + "-----------------------------------------------------------------------------" + Environment.NewLine);
            }
        }

        public static void LogLine(string line)
        {
            using (StreamWriter writer = new StreamWriter(LOG_FILE_PATH, true))
            {
                writer.WriteLine(line + Environment.NewLine + "Date :" + DateTime.Now.ToString());
                writer.WriteLine(Environment.NewLine + "-----------------------------------------------------------------------------" + Environment.NewLine);
            }
        }

    }
}
