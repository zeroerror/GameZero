using System.IO;
using UnityEditor;
using UnityEngine;

namespace GamePlay.Util
{
    public static class SpriteAtlasProcessor
    {
        public static void GenerateSeparatePNGs(Texture2D spriteAtlas)
        {
            if (spriteAtlas == null)
            {
                Debug.LogError("请先选中图集");
                return;
            }
            var path = AssetDatabase.GetAssetPath(spriteAtlas);
            string resourcePath = path.Substring(path.IndexOf("Resources") + "Resources".Length + 1);
            resourcePath = resourcePath.Substring(0, resourcePath.LastIndexOf("."));
            Sprite[] subSprites = Resources.LoadAll<Sprite>(resourcePath);
            if (subSprites == null || subSprites.Length == 0)
            {
                Debug.LogError("未能加载到任何Sprite，请确保图集在Resources文件夹内并且路径正确");
                return;
            }

            string directory = Path.GetDirectoryName(path) + "\\" + spriteAtlas.name;
            if (Directory.Exists(directory)) Directory.Delete(directory, true);
            Directory.CreateDirectory(directory);
            foreach (var subSprite in subSprites)
            {
                SaveSpriteAsPNG(subSprite, directory);
            }
        }

        private static void SaveSpriteAsPNG(Sprite sprite, string directory)
        {
            Texture2D texture = new Texture2D((int)sprite.rect.width, (int)sprite.rect.height);
            Color[] pixels = sprite.texture.GetPixels((int)sprite.rect.x, (int)sprite.rect.y, (int)sprite.rect.width, (int)sprite.rect.height);
            texture.SetPixels(pixels);
            texture.Apply();

            byte[] bytes = texture.EncodeToPNG();
            string path = Path.Combine(directory, sprite.name + ".png");
            File.WriteAllBytes(path, bytes);
            Debug.Log("保存png " + sprite.name + " to " + path);
        }
    }
}