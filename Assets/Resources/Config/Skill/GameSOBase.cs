using UnityEditor;
using UnityEngine;

namespace GamePlay.Config
{
    public abstract class GameSOBase : ScriptableObject
    {
        [Header("类型Id")]
        public int typeId;

        protected virtual void OnValidate()
        {
            EditorApplication.hierarchyChanged += RenameAssetInMainThread;
        }

        private void RenameAssetInMainThread()
        {
            EditorApplication.hierarchyChanged -= RenameAssetInMainThread; // 确保只执行一次
            RenameAssetBasedOnId();
        }

        private void RenameAssetBasedOnId()
        {
            // 获取当前文件路径
            string path = AssetDatabase.GetAssetPath(this);
            var prefix = this._getPrefix();
            string newName = $"{prefix}{typeId}";

            // 提取目录和当前文件名
            string directory = System.IO.Path.GetDirectoryName(path);
            string currentName = System.IO.Path.GetFileNameWithoutExtension(path);

            // 检查当前文件名是否已经是正确的
            if (currentName == newName) return;

            // 检查是否已经存在同名文件
            string newPath = $"{directory}/{newName}.asset";
            if (System.IO.File.Exists(newPath))
            {
                // 弹出对话框提示用户是否覆盖
                if (!EditorUtility.DisplayDialog("重命名失败", $"文件已经存在: {newName}.asset", "覆盖", "取消"))
                {
                    return;
                }
            }

            Debug.Log("SO文件重命名: " + path + " -> " + newName);
            AssetDatabase.RenameAsset(path, newName);
            AssetDatabase.SaveAssets();
        }

        protected string _getPrefix()
        {
            var atts = (CreateAssetMenuAttribute)GetType().GetCustomAttributes(typeof(CreateAssetMenuAttribute), false)[0];
            return atts.fileName;
        }
    }
}