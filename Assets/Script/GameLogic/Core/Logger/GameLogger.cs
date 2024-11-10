using UnityEngine;

namespace GamePlay.Logic.Core
{
    public class GameLogger
    {
        public static void Log(string message)
        {
            Debug.Log(message);
        }
        public static void Warning(string message)
        {
            Debug.LogWarning(message);
        }
        public static void Error(string message)
        {
            Debug.LogError(message);
        }
    }
}