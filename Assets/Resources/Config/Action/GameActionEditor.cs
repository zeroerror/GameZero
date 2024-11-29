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
            GameActionSO actionSO = (GameActionSO)target;
            // 显示通用字段
            actionSO.typeId = EditorGUILayout.IntField("类型Id", actionSO.typeId);
            actionSO.actionType = (GameActionType)EditorGUILayout.EnumPopup("行为类型", actionSO.actionType);
            // 根据行为类型动态显示字段
            switch (actionSO.actionType)
            {
                case GameActionType.Dmg:
                    EditorGUILayout.LabelField(" -------- 伤害行为 --------", EditorStyles.boldLabel);
                    this._ShowDmgAction(actionSO);
                    break;
                case GameActionType.Heal:
                    EditorGUILayout.LabelField(" -------- 治疗行为 --------", EditorStyles.boldLabel);
                    this._ShowHealAction(actionSO);
                    break;

                case GameActionType.LaunchProjectile:
                    EditorGUILayout.LabelField(" -------- 发射弹体行为 --------", EditorStyles.boldLabel);
                    this._ShowLaunchProjectileAction(actionSO);
                    break;
                default:
                    EditorGUILayout.HelpBox("请选择一个行为类型", MessageType.Warning);
                    break;
            }
            this._ShowSkillSORefs(actionSO);

            // 保存修改
            if (GUI.changed)
            {
                EditorUtility.SetDirty(actionSO);
            }
        }

        private void _ShowSkillSORefs(GameActionSO actionSO)
        {
            var color = GUI.color;
            GUI.color = Color.green;
            var skillSOs = Resources.LoadAll<GameSkillSO>(GameConfigCollection.SKILL_CONFIG_DIR_PATH);
            skillSOs = skillSOs.Filter(so => so.timelineEvents.Contains(e => e.action.typeId == actionSO.typeId));
            if (skillSOs.Length > 0)
            {
                EditorGUILayout.LabelField(" -------- 被依赖技能 --------", EditorStyles.boldLabel);
                for (int i = 0; i < skillSOs.Length; i++)
                {
                    var skillSO = skillSOs[i];
                    EditorGUILayout.ObjectField(skillSO.typeId.ToString(), skillSO, typeof(GameSkillSO), false);
                }
            }
            var projectileSOs = Resources.LoadAll<GameProjectileSO>(GameConfigCollection.BULLET_CONFIG_DIR_PATH);
            projectileSOs = projectileSOs.Filter(so => so.timelineEvents.Contains(e => e.action.typeId == actionSO.typeId));
            if (projectileSOs.Length > 0)
            {
                EditorGUILayout.LabelField(" -------- 被依赖弹体 --------", EditorStyles.boldLabel);
                for (int i = 0; i < projectileSOs.Length; i++)
                {
                    var projectileSO = projectileSOs[i];
                    EditorGUILayout.ObjectField(projectileSO.typeId.ToString(), projectileSO, typeof(GameProjectileSO), false);
                }
            }
            GUI.color = color;
        }

        private void _ShowDmgAction(GameActionSO actionSO)
        {
            GameActionModel_Dmg dmgAction = actionSO.dmgAction;
            if (dmgAction == null)
            {
                dmgAction = new GameActionModel_Dmg();
                actionSO.dmgAction = dmgAction;
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

        private void _ShowLaunchProjectileAction(GameActionSO actionSO)
        {
            GameActionModel_LaunchProjectile launchProjectileAction = actionSO.launchProjectileAction;
            if (launchProjectileAction == null)
            {
                launchProjectileAction = new GameActionModel_LaunchProjectile();
                actionSO.launchProjectileAction = launchProjectileAction;
            }
            actionSO.launchProjectileSO = (GameProjectileSO)EditorGUILayout.ObjectField("弹体模板", actionSO.launchProjectileSO, typeof(GameProjectileSO), false);
            var launchProjectileSO = actionSO.launchProjectileSO;
            launchProjectileAction.projectileId = launchProjectileSO == null ? 0 : launchProjectileSO.typeId;
            launchProjectileAction.speed = EditorGUILayout.FloatField("弹体速度", launchProjectileAction.speed);
        }
    }
}