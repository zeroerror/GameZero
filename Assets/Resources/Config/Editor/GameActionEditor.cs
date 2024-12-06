using System.Linq;
using GamePlay.Bussiness.Logic;
using GamePlay.Core;
using UnityEditor;
using UnityEngine;

namespace GamePlay.Config
{
    [CustomEditor(typeof(GameActionSO))]
    public class GameActionSOEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            GameActionSO so = (GameActionSO)target;
            EditorGUILayout.BeginVertical("box");
            this._ShowLogicData(so);
            EditorGUILayout.EndVertical();


            EditorGUILayout.BeginVertical("box");
            this._ShowRendererData(so);
            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical("box");
            this._ShowSelectorData(so);
            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical("box");
            this._ShowSkillSORefs(so);
            EditorGUILayout.EndVertical();

            if (GUI.changed)
            {
                EditorUtility.SetDirty(so);
                AssetDatabase.SaveAssets();
            }
        }

        private void _ShowLogicData(GameActionSO so)
        {
            so.typeId = EditorGUILayout.IntField("类型Id", so.typeId);
            so.actionType = (GameActionType)EditorGUILayout.EnumPopup("行为类型", so.actionType);
            if (so.actionType == GameActionType.None) EditorGUILayout.HelpBox("请选择一个行为类型", MessageType.Warning);
            _ShowAction(so);
        }

        private void _ShowAction(GameActionSO so)
        {
            var selectorEM = so.selectorEM;
            var selector = selectorEM.ToSelector();
            switch (so.actionType)
            {
                case GameActionType.Dmg:
                    EditorGUILayout.LabelField(" -------- 伤害行为 --------", EditorStyles.boldLabel);
                    this._ShowDmgAction(so);
                    so.dmgAction.selector = selector;
                    break;
                case GameActionType.Heal:
                    EditorGUILayout.LabelField(" -------- 治疗行为 --------", EditorStyles.boldLabel);
                    this._ShowHealAction(so);
                    so.healAction.selector = selector;
                    break;

                case GameActionType.LaunchProjectile:
                    EditorGUILayout.LabelField(" -------- 发射投射物行为 --------", EditorStyles.boldLabel);
                    var launchProjectileActionEM_p = serializedObject.FindProperty("launchProjectileActionEM");
                    launchProjectileActionEM_p.DrawProperty();
                    break;
                default:
                    EditorGUILayout.HelpBox("未知的行为类型", MessageType.Warning);
                    break;
            }
        }

        // 显示表现层数据
        private void _ShowRendererData(GameActionSO so)
        {
            var actionR_p = serializedObject.FindProperty("actionR");
            actionR_p.DrawProperty("表现效果");
        }

        // 显示选择器数据
        private void _ShowSelectorData(GameActionSO so)
        {
            var selectorEM_p = serializedObject.FindProperty("selectorEM");
            selectorEM_p.DrawProperty("选择器");
        }

        private void _ShowSkillSORefs(GameActionSO so)
        {
            var color = GUI.color;
            GUI.color = Color.green;
            var skillSOs = Resources.LoadAll<GameSkillSO>(GameConfigCollection.SKILL_CONFIG_DIR_PATH);
            skillSOs = skillSOs.Filter(skillSO => skillSO.timelineEvents.Contains(e => e.action?.typeId == so.typeId));
            if (skillSOs.Length > 0)
            {
                EditorGUILayout.LabelField(" -------- 被依赖技能 --------", EditorStyles.boldLabel);
                for (int i = 0; i < skillSOs.Length; i++)
                {
                    var skillSO = skillSOs[i];
                    EditorGUILayout.ObjectField(skillSO.typeId.ToString(), skillSO, typeof(GameSkillSO), false);
                }
            }
            var projectileSOs = Resources.LoadAll<GameProjectileSO>(GameConfigCollection.PROJECTILE_CONFIG_DIR_PATH);
            projectileSOs = projectileSOs.Filter(so => so.timelineEvents.Contains(e => e.action?.typeId == so.typeId));
            if (projectileSOs.Length > 0)
            {
                EditorGUILayout.LabelField(" -------- 被依赖投射物 --------", EditorStyles.boldLabel);
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
            GameActionModel_Dmg dmgAction = so.dmgAction;
            if (dmgAction == null)
            {
                dmgAction = new GameActionModel_Dmg();
                so.dmgAction = dmgAction;
            }
            dmgAction.dmg = EditorGUILayout.IntField("伤害值", dmgAction.dmg);
        }

        private void _ShowHealAction(GameActionSO actionSO)
        {
            GameActionModel_Heal healAction = actionSO.healAction;
            if (healAction == null)
            {
                healAction = new GameActionModel_Heal();
                actionSO.healAction = healAction;
            }
            healAction.heal = EditorGUILayout.IntField("治疗值", healAction.heal);
        }
    }
}