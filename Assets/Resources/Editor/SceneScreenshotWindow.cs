using UnityEditor;
using UnityEngine;

namespace GamePlay.Util
{
    public class SceneScreenshotWindow : EditorWindow
    {
        private Camera screenshotCamera;
        private int screenshotWidth = 1920;
        private int screenshotHeight = 1080;
        private string savePath = "Assets/Screenshots/screenshot.png";

        [MenuItem("Tools/Scene Screenshot")]
        public static void ShowWindow()
        {
            GetWindow<SceneScreenshotWindow>("Scene Screenshot");
        }

        private void OnGUI()
        {
            GUILayout.Label("Scene Screenshot Settings", EditorStyles.boldLabel);

            screenshotCamera = (Camera)EditorGUILayout.ObjectField("Camera", screenshotCamera, typeof(Camera), true);
            screenshotWidth = EditorGUILayout.IntField("Width", screenshotWidth);
            screenshotHeight = EditorGUILayout.IntField("Height", screenshotHeight);
            // 路劲为相机所在预制体的路径
            savePath = EditorGUILayout.TextField("Save Path", savePath);

            if (GUILayout.Button("Capture Screenshot"))
            {
                if (screenshotCamera != null)
                {
                    SceneScreenshot.CaptureScreenshot(savePath, screenshotCamera, screenshotWidth, screenshotHeight);
                }
                else
                {
                    Debug.LogError("请先选择一个摄像机");
                }
            }
        }
    }
}