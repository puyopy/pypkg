// pypupy - 2022

using System.IO;
using System.Reflection;
using pypkg;

namespace pypkg
{
    public class Logger
    {
        public enum InfoType
        {
            Info,
            Warn,
            Debug,
            Exception,
        }

        private static readonly string ExecutionDirectory = Assembly.GetExecutingAssembly().Location;
        private static object[] Logs = Array.Empty<object>();

        // Logs a message.
        public static void Log(object message, InfoType type = InfoType.Info)
        {
            switch (type)
            {
                case InfoType.Info:
                    message = "[INFO] " + message;
                    break;
                case InfoType.Warn:
                    message = "[WARN] " + message;
                    break;
                case InfoType.Debug:
                    message = "[DEBUG] " + message;
                    break;
            }
            AddToLogs(message);
            Console.WriteLine("[pypkg] " + message);
        }
        
        // Adds a brand new entry to the logs array.
        private static void AddToLogs(object Message)
        {
            Logs.Append(Message);
        }

        // messy way of logging all possible objects... dang.
        public static async void DumpLogs()
        {
            string Output = "";
            
            foreach (object msg in Logs)
            {
                Output += msg.ToString() + "\n";
            }

            string CurrentDate = DateTime.Now.ToString();
            string Path = ExecutionDirectory + CurrentDate + "-crash-log.txt";            
            try
            {

                await File.WriteAllTextAsync(Path, Output);
                Log("Dumped crash logs in " + Path,InfoType.Debug);
            } catch (Exception err)
            {
                Log("Failed to dump crash logs: " +err,InfoType.Exception);
            }
        }
    }
}
