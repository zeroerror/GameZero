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
        private SerializedProperty actionType_p;
        private SerializedProperty dmgActionEM_p;
        private SerializedProperty healActionEM_p;
        private SerializedProperty attributeModifyActionEM_p;
        private SerializedProperty launchProjectileActionEM_p;
        private SerializedProperty knockBackActionEM_p;
        private SerializedProperty attachBuffActionEM_p;
        private SerializedProperty detachBuffActionEM_p;
        private SerializedProperty summonRolesActionEM_p;
        private SerializedProperty characterTransformActionEM_p;
        private SerializedProperty stealthActionEM_p;

        private SerializedProperty actionEMR_p;

        private void OnEnable()
        {
            this._serializedObject = new SerializedObject(target);
            this.typeId_p = _serializedObject.FindProperty("typeId");
            this.actionType_p = _serializedObject.FindProperty("actionType");
            this.dmgActionEM_p = _serializedObject.FindProperty("dmgActionEM");
            this.healActionEM_p = _serializedObject.FindProperty("healActionEM");
            this.attributeModifyActionEM_p = _serializedObject.FindProperty("attributeModifyActionEM");
            this.launchProjectileActionEM_p = _serializedObject.FindProperty("launchProjectileActionEM");
            this.knockBackActionEM_p = _serializedObject.FindProperty("knockBackActionEM");
            this.attachBuffActionEM_p = _serializedObject.FindProperty("attachBuffActionEM");
            this.detachBuffActionEM_p = _serializedObject.FindProperty("detachBuffActionEM");
            this.summonRolesActionEM_p = _serializedObject.FindProperty("summonRolesActionEM");
            this.characterTransformActionEM_p = _serializedObject.FindProperty("characterTransformActionEM");
            this.stealthActionEM_p = _serializedObject.FindProperty("stealthActionEM");

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
                    EditorGUILayout.LabelField(" -------- 伤害 --------", EditorStyles.boldLabel);
                    this._ShowDmgAction(so);
                    so.dmgActionEM.selectorEM = selectorEM;
                    break;
                case GameActionType.Heal:
                    EditorGUILayout.LabelField(" -------- 治疗 --------", EditorStyles.boldLabel);
                    this._ShowHealAction(so);
                    so.healActionEM.selectorEM = selectorEM;
                    break;
                case GameActionType.AttributeModify:
                    EditorGUILayout.LabelField(" -------- 属性修改(+/-) --------", EditorStyles.boldLabel);
                    this._ShowAttributeModifyAction(so);
                    so.attributeModifyActionEM.selectorEM = selectorEM;
                    break;
                case GameActionType.LaunchProjectile:
                    EditorGUILayout.LabelField(" -------- 发射投射物 --------", EditorStyles.boldLabel);
                    launchProjectileActionEM_p.DrawProperty();
                    break;
                case GameActionType.KnockBack:
                    EditorGUILayout.LabelField(" -------- 击退 --------", EditorStyles.boldLabel);
                    knockBackActionEM_p.DrawProperty();
                    break;
                case GameActionType.AttachBuff:
                    EditorGUILayout.LabelField(" -------- 附加Buff --------", EditorStyles.boldLabel);
                    attachBuffActionEM_p.DrawProperty();
                    break;
                case GameActionType.DetachBuff:
                    EditorGUILayout.LabelField(" -------- 移除Buff --------", EditorStyles.boldLabel);
                    detachBuffActionEM_p.DrawProperty();
                    break;
                case GameActionType.SummonRoles:
                    EditorGUILayout.LabelField(" -------- 召唤角色 --------", EditorStyles.boldLabel);
                    summonRolesActionEM_p.DrawProperty();
                    break;
                case GameActionType.CharacterTransform:
                    EditorGUILayout.LabelField(" -------- 角色变身 --------", EditorStyles.boldLabel);
                    characterTransformActionEM_p.DrawProperty();
                    break;
                case GameActionType.Stealth:
                    EditorGUILayout.LabelField(" -------- 隐身 --------", EditorStyles.boldLabel);
                    stealthActionEM_p.DrawProperty();
                    break;
                default:
                    EditorGUILayout.HelpBox($"未知的行为类型：{so.actionType}", MessageType.Warning);
                    break;
            }
        }

        // 显示表现层数据
        private void _ShowRendererData(GameActionSO so)
        {
            actionEMR_p.DrawProperty("表现效果");
        }

        private void _ShowSkillSORefs(GameActionSO actionSO)
        {
            var color = GUI.color;
            GUI.color = Color.green;
            var actionId = actionSO.typeId;

            var skillSOs = Resources.LoadAll<GameSkillSO>(GameConfigCollection.SKILL_CONFIG_DIR_PATH);
            skillSOs = skillSOs?.Filter(skillSO => skillSO.timelineEvents.Contains(ev => ev.actions?.Find(a => a.typeId == actionId) != null));
            if (skillSOs?.Length > 0)
            {
                EditorGUILayout.LabelField(" -------- 被以下技能使用 --------", EditorStyles.boldLabel);
                for (int i = 0; i < skillSOs.Length; i++)
                {
                    var skillSO = skillSOs[i];
                    EditorGUILayout.ObjectField(skillSO.typeId.ToString(), skillSO, typeof(GameSkillSO), false);
                }
            }

            var projectileSOs = Resources.LoadAll<GameProjectileSO>(GameConfigCollection.PROJECTILE_CONFIG_DIR_PATH);
            projectileSOs = projectileSOs?.Filter((pjSO) =>
            {
                if (pjSO.timelineEvents != null && pjSO.timelineEvents.Contains(e => e.actions?.Contains(a => a.typeId == actionId) == true))
                {
                    return true;
                }
                if (pjSO.stateEMs != null && pjSO.stateEMs.Contains(s => s.emSet.HasRefAction(actionId)))
                {
                    return true;
                }
                return false;
            }
            );
            if (projectileSOs?.Length > 0)
            {
                EditorGUILayout.LabelField(" -------- 被以下投射物使用 --------", EditorStyles.boldLabel);
                for (int i = 0; i < projectileSOs.Length; i++)
                {
                    var projectileSO = projectileSOs[i];
                    EditorGUILayout.ObjectField(projectileSO.desc, projectileSO, typeof(GameProjectileSO), false);
                }
            }

            var buffSOs = Resources.LoadAll<GameBuffSO>(GameConfigCollection.BUFF_CONFIG_DIR_PATH);
            buffSOs = buffSOs?.Filter(buffSO => buffSO.actionSOs?.Contains(a => a?.typeId == actionId) == true);
            if (buffSOs?.Length > 0)
            {
                EditorGUILayout.LabelField(" -------- 被以下Buff使用 --------", EditorStyles.boldLabel);
                for (int i = 0; i < buffSOs.Length; i++)
                {
                    var buffSO = buffSOs[i];
                    EditorGUILayout.ObjectField(buffSO.desc, buffSO, typeof(GameBuffSO), false);
                }
            }

            var actionOptionSOs = Resources.LoadAll<GameActionOptionSO>(GameConfigCollection.ACTION_OPTION_CONFIG_DIR_PATH);
            actionOptionSOs = actionOptionSOs?.Filter(actionOptionSO => actionOptionSO.actionSOs?.Contains(a => a?.typeId == actionId) == true);
            if (actionOptionSOs?.Length > 0)
            {
                EditorGUILayout.LabelField(" -------- 被以下ActionOption使用 --------", EditorStyles.boldLabel);
                for (int i = 0; i < actionOptionSOs.Length; i++)
                {
                    var actionOptionSO = actionOptionSOs[i];
                    var tips = $"选项[{actionOptionSO.typeId}]: {actionOptionSO.desc}";
                    EditorGUILayout.ObjectField(tips, actionOptionSO, typeof(GameActionOptionSO), false);
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

        private void _ShowAttributeModifyAction(GameActionSO actionSO)
        {
            if (attributeModifyActionEM_p == null)
            {
                attributeModifyActionEM_p = new SerializedObject(actionSO).FindProperty("attributeModifyActionEM");
            }
            attributeModifyActionEM_p.DrawProperty();
        }
    }
}