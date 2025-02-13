using UnityEditor;
using UnityEngine;

namespace GamePlay.Util
{
    public class SpriteAtlasProcessorWindow : EditorWindow
    {
        private Texture2D spriteAtlas;

        [MenuItem("Tools/图集分离")]
        public static void ShowWindow()
        {
            GetWindow<SpriteAtlasProcessorWindow>("图集分离");
        }

        private void OnGUI()
        {
            GUILayout.Label("图集分离", EditorStyles.boldLabel);

            spriteAtlas = (Texture2D)EditorGUILayout.ObjectField("Sprite Atlas", spriteAtlas, typeof(Texture2D), false);
            if (spriteAtlas == null)
            {
                Debug.LogError("请先选中图集");
                return;
            }

            // 默认设置图集为可读
            string path = AssetDatabase.GetAssetPath(spriteAtlas);
            TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;
            if (importer != null && !importer.isReadable)
            {
                importer.isReadable = true;
                AssetDatabase.ImportAsset(path);
            }

            if (GUILayout.Button("生成分离的PNG"))
            {
                SpriteAtlasProcessor.GenerateSeparatePNGs(spriteAtlas);
            }
        }
    }
}