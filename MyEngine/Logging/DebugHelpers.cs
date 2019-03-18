using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace MyEngine.Logging
{
    public class Logger
    {
        public ConcurrentQueue<LogMessage> loggingQueue = new ConcurrentQueue<LogMessage>();
        private Thread LoggingTask;
        private bool _doLog;

        public Logger()
        {
            LoggingTask = new Thread(new ThreadStart(Logging));
            _doLog = false;
        }
        private void Logging()
        {
            while (_doLog)
            {
                LogMessage logMessage;
                if(!loggingQueue.TryDequeue(out logMessage))
                    continue;
                if (logMessage.Title != "" && logMessage.Message != "")
                {
                    DebugHelpers.Log(logMessage.Title,logMessage.Message);
                }
                else if (logMessage.Message != "")
                {
                    DebugHelpers.Log(logMessage.Message);
                }

            }
        }

        public void Start()
        {
            _doLog = true;
            LoggingTask.Start();
        }

        public void Stop()
        {
            if(!_doLog) return;
            _doLog = false;
            LoggingTask.Join();
        }

        public void Log(LogMessage value)
        {
            loggingQueue.Enqueue(value);
        }
    }
    public static class DebugHelpers
    {

        internal static void Log(string title, string message)
        {
            
            if (title.Contains("<clear>"))
            {
                Console.Clear();
            }
            Console.WriteLine("\n"+ title.Replace("<clear>",""));
            Console.WriteLine(String.Empty.PadLeft(Console.BufferWidth - 1, '-'));
            Console.Write(message);
            Console.WriteLine(Environment.NewLine+String.Empty.PadLeft(Console.BufferWidth - 1, '=')+ Environment.NewLine);
        }
        internal static void Log(string value)
        {
            if (value.Contains("<clear>"))
            {
                Console.Clear();
                value.Replace("<clear>","");
            }
            Console.WriteLine(value);
        }
    }

    public struct LogMessage
    {
       public string Title;
       public string Message;
       public LogMessage(string message, string title = "")
       {
           Title = title;
           Message = message;
       }
    }
}
