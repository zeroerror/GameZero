using System.IO;
using UnityEngine;
namespace GamePlay.Util
{
    public static class SceneScreenshot
    {
        public static void CaptureScreenshot(string path, Camera camera, int width, int height)
        {
            RenderTexture renderTexture = new RenderTexture(width, height, 24);
            var oldtargetTexture = camera.targetTexture;
            camera.targetTexture = renderTexture;
            camera.Render();
            var oldActiveTexture = RenderTexture.active;
            RenderTexture.active = renderTexture;
            Texture2D screenShot = new Texture2D(width, height, TextureFormat.RGB24, false);
            screenShot.ReadPixels(new Rect(0, 0, width, height), 0, 0);
            screenShot.Apply();

            byte[] bytes = screenShot.EncodeToPNG();
            File.WriteAllBytes(path, bytes);
            Debug.Log("Screenshot saved to: " + path);

            camera.targetTexture = oldtargetTexture;
            RenderTexture.active = oldActiveTexture;
        }
    }
}