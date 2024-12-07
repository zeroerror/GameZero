using System.Linq;
using GamePlay.Bussiness.Logic;
using GamePlay.Core;
using UnityEditor;
using UnityEngine;

namespace GamePlay.Config
{
    [CustomEditor(typeof(GameSkillSO))]
    public class GameEditor_Skill : Editor
    {
        public override void OnInspectorGUI()
        {
            EditorGUI.BeginChangeCheck();
            GameSkillSO so = (GameSkillSO)target;

            EditorGUILayout.BeginVertical("box");
            this._ShowBasicData(so);
            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical("box");
            this._ShowRoleSORefs(so);
            EditorGUILayout.EndVertical();

            if (EditorGUI.EndChangeCheck())
            {
                EditorUtility.SetDirty(so);
            }
        }

        private void _ShowBasicData(GameSkillSO so)
        {
            EditorGUILayout.LabelField("基础信息-------------------------", EditorStyles.boldLabel);
            so.typeId = EditorGUILayout.IntField("类型Id", so.typeId);
            so.desc = EditorGUILayout.TextField("描述", so.desc);
            EditorGUI.BeginChangeCheck();
            so.animClip = (AnimationClip)EditorGUILayout.ObjectField("动画文件", so.animClip, typeof(AnimationClip), false);
            if (EditorGUI.EndChangeCheck())
            {
                so.Update();
            }
            EditorGUILayout.LabelField("动画名称", so.animName);
            EditorGUILayout.LabelField("动画时长(s)", so.animLength.ToString());
            var timelineEvents_p = serializedObject.FindProperty("timelineEvents");
            timelineEvents_p.DrawProperty();
            var conditionEM_p = serializedObject.FindProperty("conditionEM");
            conditionEM_p.DrawProperty();
        }

        private void _ShowRoleSORefs(GameSkillSO so)
        {
            var color = GUI.color;
            GUI.color = Color.green;
            var roleSOs = Resources.LoadAll<GameRoleSO>(GameConfigCollection.ROLE_CONFIG_DIR_PATH);
            roleSOs = roleSOs.Filter(roleSO => roleSO.skills.Contains(roleSkillSO => roleSkillSO.typeId == so.typeId));
            if (roleSOs.Length > 0)
            {
                EditorGUILayout.LabelField(" -------- 被依赖执行的角色 --------", EditorStyles.boldLabel);
                for (int i = 0; i < roleSOs.Length; i++)
                {
                    var skillSO = roleSOs[i];
                    EditorGUILayout.ObjectField(skillSO.typeId.ToString(), skillSO, typeof(GameSkillSO), false);
                }
            }
            GUI.color = color;
        }


    }
}