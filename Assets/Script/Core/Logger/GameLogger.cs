using UnityEngine;

namespace GamePlay.Core
{
    public enum LogLevel
    {
        Info = 0,
        Warning,
        Error
    }
    public class GameLogger
    {
        public static LogLevel logLevel = LogLevel.Warning;
        public static void Log(string message)
        {
            if (logLevel > LogLevel.Info) return;
            Debug.Log(message);
        }
        public static void LogWarning(string message)
        {
            if (logLevel > LogLevel.Warning) return;
            Debug.LogWarning(message);
        }
        public static void LogError(string message)
        {
            if (logLevel > LogLevel.Error) return;
            Debug.LogError(message);
        }
    }
}