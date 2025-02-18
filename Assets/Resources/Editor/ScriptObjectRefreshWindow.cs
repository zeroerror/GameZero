using GamePlay.Config;
using UnityEditor;
using UnityEngine;

namespace GamePlay.Util
{
    public static class GameSORefreshWindow
    {
        [MenuItem("Tools/刷新保存所有技能模板数据")]
        public static void RefreshTemplate_Skill()
        {
            var sos = Resources.LoadAll<GameSkillSO>(GameConfigCollection.SKILL_CONFIG_DIR_PATH);
            foreach (var so in sos)
            {
                var editor = Editor.CreateEditor(so) as GameEditor_Skill;
                editor.RefreshClipInfo(true);
            }
            AssetDatabase.SaveAssets();
        }
    }
}