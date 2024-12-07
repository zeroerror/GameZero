using UnityEditor;
using UnityEngine;

namespace GamePlay.Config
{
    public static class GameEditorGUILayout
    {
        private static Texture2D DefaultBoxTexture1
        {
            get
            {
                if (_defaultBoxTexture == null)
                {
                    _defaultBoxTexture = new Texture2D(1, 1);
                    _defaultBoxTexture.SetPixel(0, 0, new Color(0.0f, 0.0f, 0.0f, 0.3f));
                    _defaultBoxTexture.Apply();
                }
                return _defaultBoxTexture;
            }
        }
        private static Texture2D _defaultBoxTexture;


        public static void DrawBoxItem(System.Action action, Texture2D tex2d = null)
        {
            GUIStyle boxStyle = new GUIStyle(GUI.skin.box);
            boxStyle.normal.background = tex2d ?? DefaultBoxTexture1;
            boxStyle.normal.textColor = Color.white;
            EditorGUILayout.BeginVertical(boxStyle);
            action();
            EditorGUILayout.EndVertical();
        }
    }
}