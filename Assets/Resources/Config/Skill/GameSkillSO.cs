#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Callbacks;
#endif
using UnityEngine;

namespace GamePlay.Config
{
    [CreateAssetMenu(fileName = "template_skill_", menuName = "游戏玩法/配置/技能模板")]
    public class GameSkillSO : ScriptableObject
    {
        [Header("类型Id")]
        public int typeId;
        [Header("动画文件")]
        public AnimationClip animClip;
        [Header("动画名称")]
        public string animName;
        [Header("动画时长(s)")]
        public float animLength;

        private void OnValidate()
        {
            // 更新动画信息
            if (animClip != null)
            {
                animName = animClip.name;
                animLength = animClip.length;
            }

#if UNITY_EDITOR
            EditorApplication.delayCall += RenameAssetBasedOnId;
#endif
        }

#if UNITY_EDITOR
        private void RenameAssetBasedOnId()
        {
            // 获取当前文件路径
            string path = AssetDatabase.GetAssetPath(this);
            string newName = $"template_skill_{typeId}";

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

            // 调用 Unity 的 AssetDatabase API 修改文件名
            AssetDatabase.RenameAsset(path, newName);
            AssetDatabase.SaveAssets();

            // 取消挂载的回调
            EditorApplication.delayCall -= RenameAssetBasedOnId;
        }
#endif
    }
}
