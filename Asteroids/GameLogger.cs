using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;

namespace Asteroids
{
    delegate void Logger(string str);
    public class GameLogger
    {
        public static void ConsoleLogger(string message)
        {
            Console.WriteLine(DateTime.Now + ": " + message);
        }

        public static void DebugLogger(string message)
        {
            Debug.WriteLine(DateTime.Now + ": " + message);
        }

        public static void FileLogger(string message)
        {
            using (StreamWriter streamWriter = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "log.txt", true))
            {
                streamWriter.WriteLine(DateTime.Now + ": " + message);
            }
        }
    }
}
