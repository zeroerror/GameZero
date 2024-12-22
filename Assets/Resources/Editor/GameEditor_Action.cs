using System.Linq;
using GamePlay.Bussiness.Logic;
using GamePlay.Core;
using UnityEditor;
using UnityEngine;

namespace GamePlay.Config
{
    [CustomEditor(typeof(GameActionSO))]
    public class GameEditor_Action : Editor
    {
        private SerializedObject _serializedObject;
        private SerializedProperty typeId_p;
        private SerializedProperty desc_p;
        private SerializedProperty actionType_p;
        private SerializedProperty dmgActionEM_p;
        private SerializedProperty healActionEM_p;
        private SerializedProperty launchProjectileActionEM_p;
        private SerializedProperty knockBackActionEM_p;
        private SerializedProperty attachBuffActionEM_p;

        private SerializedProperty actionEMR_p;

        private void OnEnable()
        {
            this._serializedObject = new SerializedObject(target);
            this.typeId_p = _serializedObject.FindProperty("typeId");
            this.desc_p = _serializedObject.FindProperty("desc");
            this.actionType_p = _serializedObject.FindProperty("actionType");
            this.dmgActionEM_p = _serializedObject.FindProperty("dmgActionEM");
            this.healActionEM_p = _serializedObject.FindProperty("healActionEM");
            this.launchProjectileActionEM_p = _serializedObject.FindProperty("launchProjectileActionEM");
            this.knockBackActionEM_p = _serializedObject.FindProperty("knockBackActionEM");
            this.attachBuffActionEM_p = _serializedObject.FindProperty("attachBuffActionEM");

            this.actionEMR_p = _serializedObject.FindProperty("actionEMR");
        }

        public override void OnInspectorGUI()
        {
            this._serializedObject.Update();

            GameActionSO so = (GameActionSO)target;
            EditorGUILayout.BeginVertical("box");
            this._ShowLogicData(so);
            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical("box");
            this._ShowRendererData(so);
            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical("box");
            this._ShowSkillSORefs(so);
            EditorGUILayout.EndVertical();

            this._serializedObject.ApplyModifiedProperties();
        }

        private void _ShowLogicData(GameActionSO so)
        {
            EditorGUILayout.BeginVertical("box");
            this.typeId_p.DrawProperty_Int("类型ID");
            this.desc_p.DrawProperty_Str("描述");
            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical("box");
            var actionType = this.actionType_p.DrawProperty_EnumPopup<GameActionType>("行为类型");
            if (actionType == GameActionType.None) EditorGUILayout.HelpBox("请选择一个行为类型", MessageType.Warning);
            _ShowAction(so);
            EditorGUILayout.EndVertical();
        }

        private void _ShowAction(GameActionSO so)
        {
            var selectorEM = so.GetCurSelectorEM();
            switch (so.actionType)
            {
                case GameActionType.Dmg:
                    EditorGUILayout.LabelField(" -------- 伤害行为 --------", EditorStyles.boldLabel);
                    this._ShowDmgAction(so);
                    so.dmgActionEM.selectorEM = selectorEM;
                    break;
                case GameActionType.Heal:
                    EditorGUILayout.LabelField(" -------- 治疗行为 --------", EditorStyles.boldLabel);
                    this._ShowHealAction(so);
                    so.healActionEM.selectorEM = selectorEM;
                    break;

                case GameActionType.LaunchProjectile:
                    EditorGUILayout.LabelField(" -------- 发射投射物行为 --------", EditorStyles.boldLabel);
                    launchProjectileActionEM_p.DrawProperty();
                    break;
                case GameActionType.KnockBack:
                    EditorGUILayout.LabelField(" -------- 击退行为 --------", EditorStyles.boldLabel);
                    knockBackActionEM_p.DrawProperty();
                    break;
                case GameActionType.AttachBuff:
                    EditorGUILayout.LabelField(" -------- 附加Buff行为 --------", EditorStyles.boldLabel);
                    attachBuffActionEM_p.DrawProperty();
                    break;
                default:
                    EditorGUILayout.HelpBox("未知的行为类型", MessageType.Warning);
                    break;
            }
        }

        // 显示表现层数据
        private void _ShowRendererData(GameActionSO so)
        {
            actionEMR_p.DrawProperty("表现效果");
        }

        private void _ShowSkillSORefs(GameActionSO so)
        {
            var color = GUI.color;
            GUI.color = Color.green;
            var skillSOs = Resources.LoadAll<GameSkillSO>(GameConfigCollection.SKILL_CONFIG_DIR_PATH);
            skillSOs = skillSOs.Filter(skillSO => skillSO.timelineEvents.Contains(e => e.actions?.Find(a => a.typeId == so.typeId) != null));
            if (skillSOs.Length > 0)
            {
                EditorGUILayout.LabelField(" -------- 被依赖执行的技能 --------", EditorStyles.boldLabel);
                for (int i = 0; i < skillSOs.Length; i++)
                {
                    var skillSO = skillSOs[i];
                    EditorGUILayout.ObjectField(skillSO.typeId.ToString(), skillSO, typeof(GameSkillSO), false);
                }
            }
            var projectileSOs = Resources.LoadAll<GameProjectileSO>(GameConfigCollection.PROJECTILE_CONFIG_DIR_PATH);
            projectileSOs = projectileSOs.Filter(so => so.timelineEvents.Contains(e => e.actions?.Find(a => a.typeId == so.typeId) != null));
            if (projectileSOs.Length > 0)
            {
                EditorGUILayout.LabelField(" -------- 被依赖执行的投射物 --------", EditorStyles.boldLabel);
                for (int i = 0; i < projectileSOs.Length; i++)
                {
                    var projectileSO = projectileSOs[i];
                    EditorGUILayout.ObjectField(projectileSO.typeId.ToString(), projectileSO, typeof(GameProjectileSO), false);
                }
            }
            GUI.color = color;
        }

        private void _ShowDmgAction(GameActionSO so)
        {
            if (dmgActionEM_p == null)
            {
                dmgActionEM_p = new SerializedObject(so).FindProperty("dmgActionEM");
            }
            dmgActionEM_p.DrawProperty();
        }

        private void _ShowHealAction(GameActionSO actionSO)
        {
            if (healActionEM_p == null)
            {
                healActionEM_p = new SerializedObject(actionSO).FindProperty("healActionEM");
            }
            healActionEM_p.DrawProperty();
        }
    }
}