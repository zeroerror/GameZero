using UnityEngine;

namespace GamePlay.Config
{
    public static class GameGUILayout
    {
        public static void DrawButton(string text, System.Action onClick, Color bgColor = default, int width = 50)
        {
            var color = GUI.backgroundColor;
            GUI.backgroundColor = bgColor;
            if (GUILayout.Button(text, GUILayout.Width(width)))
            {
                onClick?.Invoke();
            }
            GUI.backgroundColor = color;
        }
    }
}