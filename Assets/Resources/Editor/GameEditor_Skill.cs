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
        private SerializedObject _serializedObject;
        private SerializedProperty typeId_p;
        private SerializedProperty desc_p;
        private SerializedProperty skillType_p;
        private SerializedProperty animClip_p;
        private SerializedProperty animName_p;
        private SerializedProperty animLength_p;
        private SerializedProperty timelineEvents_p;
        private SerializedProperty conditionEM_p;
        private void OnEnable()
        {
            this._serializedObject = new SerializedObject(target);
            this.typeId_p = _serializedObject.FindProperty("typeId");
            this.desc_p = _serializedObject.FindProperty("desc");
            this.skillType_p = _serializedObject.FindProperty("skillType");
            this.animClip_p = _serializedObject.FindProperty("animClip");
            this.animName_p = _serializedObject.FindProperty("animName");
            this.animLength_p = _serializedObject.FindProperty("animLength");
            this.timelineEvents_p = _serializedObject.FindProperty("timelineEvents");
            this.conditionEM_p = _serializedObject.FindProperty("conditionEM");
        }

        public override void OnInspectorGUI()
        {
            this._serializedObject.Update();

            GameSkillSO so = (GameSkillSO)target;

            EditorGUILayout.BeginVertical("box");
            this._ShowBasicData(so);
            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical("box");
            this._ShowRoleSORefs(so);
            EditorGUILayout.EndVertical();

            this._serializedObject.ApplyModifiedProperties();
        }

        private void _ShowBasicData(GameSkillSO so)
        {
            EditorGUILayout.LabelField("基础信息-------------------------", EditorStyles.boldLabel);
            this.typeId_p.DrawProperty_Int("类型Id");
            this.desc_p.DrawProperty("描述");
            this.skillType_p.DrawProperty("技能类型");
            this.animClip_p.DrawProperty("动画文件");
            this.animName_p.DrawProperty("动画名称");
            this.animLength_p.DrawProperty("动画时长(s)");
            this.timelineEvents_p.DrawProperty();
            this.conditionEM_p.DrawProperty();

            var clip = this.animClip_p.objectReferenceValue as AnimationClip;
            if (clip && GUI.changed)
            {
                // 同步动画名称和时长
                this.animName_p.stringValue = clip.name;
                this.animLength_p.floatValue = clip.length;
                // 同步时间轴事件
                var events = AnimationUtility.GetAnimationEvents(clip);
                this.timelineEvents_p.arraySize = events?.Length ?? 0;
                for (int i = 0; i < this.timelineEvents_p.arraySize; i++)
                {
                    var e = events[i];
                    var time = e.time;
                    var frame = (int)(e.time * GameTimeCollection.frameRate);
                    var element = this.timelineEvents_p.GetArrayElementAtIndex(i);
                    if (element == null)
                    {
                        this.timelineEvents_p.InsertArrayElementAtIndex(i);
                        element = this.timelineEvents_p.GetArrayElementAtIndex(i);
                    }
                    element.FindPropertyRelative("time").floatValue = time;
                    element.FindPropertyRelative("frame").intValue = frame;
                }
            }
        }

        private void _ShowRoleSORefs(GameSkillSO so)
        {
            var color = GUI.color;
            GUI.color = Color.green;
            var roleSOs = Resources.LoadAll<GameRoleSO>(GameConfigCollection.ROLE_CONFIG_DIR_PATH);
            roleSOs = roleSOs.Filter(roleSO => roleSO.skills.Contains(roleSkillSO => roleSkillSO.typeId == so.typeId));
            if (roleSOs.Length > 0)
            {
                EditorGUILayout.LabelField(" -------- 被以下角色使用 --------", EditorStyles.boldLabel);
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