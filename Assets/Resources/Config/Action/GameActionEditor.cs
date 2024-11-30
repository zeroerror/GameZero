using System.Linq;
using GamePlay.Bussiness.Logic;
using GamePlay.Core;
using Unity.VisualScripting;
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
            this._ShowLogicData(so);
            this._ShowRendererData(so);
            this._ShowSkillSORefs(so);
            // 保存修改
            if (GUI.changed)
            {
                EditorUtility.SetDirty(so);
                AssetDatabase.SaveAssets();
            }
        }

        // 显示逻辑层数据
        private void _ShowLogicData(GameActionSO so)
        {
            so.typeId = EditorGUILayout.IntField("类型Id", so.typeId);
            so.actionType_edit = (GameActionType)EditorGUILayout.EnumPopup("行为类型", so.actionType_edit);
            if (so.actionType_edit == GameActionType.None) EditorGUILayout.HelpBox("请选择一个行为类型", MessageType.Warning);

            // 选择器
            EditorGUILayout.LabelField(" -------- 选择器 --------", EditorStyles.boldLabel);
            var selector = so.selector_edit;
            selector.selectAnchorType = (GameEntitySelectAnchorType)EditorGUILayout.EnumPopup("选择锚点类型", selector.selectAnchorType);
            if (selector.selectAnchorType == GameEntitySelectAnchorType.None) EditorGUILayout.HelpBox("请选择一个选择锚点类型", MessageType.Warning);
            selector.campType = (GameCampType)EditorGUILayout.EnumPopup("阵营类型", selector.campType);
            if (selector.campType == GameCampType.None) EditorGUILayout.HelpBox("请选择一个阵营类型", MessageType.Warning);
            selector.entityType = (GameEntityType)EditorGUILayout.EnumPopup("实体类型", selector.entityType);
            if (selector.entityType == GameEntityType.None) EditorGUILayout.HelpBox("请选择一个实体类型", MessageType.Warning);
            so.isRangeSelect = EditorGUILayout.Toggle("是否范围选择", so.isRangeSelect);
            if (so.isRangeSelect)
            {
                // 获取选中的GameObject
                var go = Selection.activeGameObject;
                EditorGUILayout.HelpBox("可在场景中选择碰撞器来读取参数", MessageType.Info);
                var collider = go?.GetComponent<Collider>();
                if (!collider) go = null;

                var color = GUI.color;
                GUI.color = Color.green;
                if (go) EditorGUILayout.ObjectField("当前选中", go, typeof(GameObject), true);
                GUI.color = color;

                if (!go)
                {
                    switch (so.colliderType_edit)
                    {
                        case GameColliderType.Box:
                            this._ShowBoxModel(so.boxColliderModel);
                            break;
                        case GameColliderType.Circle:
                            this._ShowCircleModel(so.circleColliderModel);
                            break;
                        case GameColliderType.Fan:
                            this._ShowFanModel(so.fanColliderModel);
                            break;
                        default:
                            EditorGUILayout.HelpBox("未知的碰撞体类型", MessageType.Warning);
                            break;
                    }
                }
                else
                {
                    if (collider) so.colliderType_edit =
                    collider is BoxCollider ? GameColliderType.Box :
                    collider is SphereCollider ? GameColliderType.Circle :
                    GameColliderType.Fan;
                    var trans = go?.transform;
                    var lossyScale = trans?.lossyScale != null ? trans.lossyScale : Vector3.one;
                    var angle = trans?.eulerAngles.z != null ? trans.eulerAngles.z : 0;
                    switch (so.colliderType_edit)
                    {
                        case GameColliderType.Box:
                            var boxColliderModel = so.boxColliderModel != null ? so.boxColliderModel : new GameBoxColliderModel(Vector2.zero, 0, 0, 0);
                            so.boxColliderModel = boxColliderModel;
                            boxColliderModel.offset = trans.position;
                            boxColliderModel.angle = angle;
                            boxColliderModel.width = lossyScale.x;
                            boxColliderModel.height = lossyScale.y;
                            this._ShowBoxModel(boxColliderModel);
                            break;
                        case GameColliderType.Circle:
                            var circleColliderModel = so.circleColliderModel != null ? so.circleColliderModel : new GameCircleColliderModel(Vector2.zero, 0, 0);
                            so.circleColliderModel = circleColliderModel;
                            circleColliderModel.offset = trans.position;
                            circleColliderModel.angle = angle;
                            circleColliderModel.radius = lossyScale.x;
                            this._ShowCircleModel(circleColliderModel);
                            break;
                        case GameColliderType.Fan:
                            var fanColliderModel = so.fanColliderModel != null ? so.fanColliderModel : new GameFanColliderModel(Vector2.zero, 0, 0, 0);
                            so.fanColliderModel = fanColliderModel;
                            fanColliderModel.offset = trans.position;
                            fanColliderModel.angle = angle;
                            fanColliderModel.radius = lossyScale.x;
                            EditorGUILayout.Vector2Field("偏移", fanColliderModel.offset);
                            EditorGUILayout.FloatField("角度", fanColliderModel.angle);
                            fanColliderModel.fanAngle = EditorGUILayout.FloatField("扇形角度", fanColliderModel.fanAngle);
                            EditorGUILayout.FloatField("半径", fanColliderModel.radius);
                            break;
                        default:
                            EditorGUILayout.HelpBox("未处理的碰撞体类型", MessageType.Warning);
                            break;
                    }
                }
            }
            so.selector_edit = selector;

            // 根据行为类型动态显示字段
            switch (so.actionType_edit)
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
                    EditorGUILayout.LabelField(" -------- 发射弹体行为 --------", EditorStyles.boldLabel);
                    this._ShowLaunchProjectileAction(so);
                    so.launchProjectileAction.selector = selector;
                    break;
                default:
                    EditorGUILayout.HelpBox("未知的行为类型", MessageType.Warning);
                    break;
            }
        }

        private void _ShowBoxModel(GameBoxColliderModel model)
        {
            if (model == null) return;
            EditorGUILayout.Vector2Field("偏移", model.getoffset);
            EditorGUILayout.FloatField("角度", model.getangle);
            EditorGUILayout.FloatField("宽度", model.width);
            EditorGUILayout.FloatField("高度", model.height);
        }

        private void _ShowCircleModel(GameCircleColliderModel model)
        {
            if (model == null) return;
            EditorGUILayout.Vector2Field("偏移", model.getoffset);
            EditorGUILayout.FloatField("角度", model.getangle);
            EditorGUILayout.FloatField("半径", model.radius);
        }

        private void _ShowFanModel(GameFanColliderModel model)
        {
            if (model == null) return;
            EditorGUILayout.Vector2Field("偏移", model.getoffset);
            EditorGUILayout.FloatField("角度", model.getangle);
            EditorGUILayout.FloatField("扇形角度", model.fanAngle);
            EditorGUILayout.FloatField("半径", model.radius);
        }

        // 显示表现层数据
        private void _ShowRendererData(GameActionSO so)
        {
            var actionR = so.actionR;
            actionR.typeId = so.typeId;
            actionR.vfxClip = (AnimationClip)EditorGUILayout.ObjectField("特效", actionR.vfxClip, typeof(AnimationClip), false);
            var shakeModel = actionR.shakeModel;
            shakeModel.angle = EditorGUILayout.FloatField("震屏角度", shakeModel.angle);
            shakeModel.amplitude = EditorGUILayout.FloatField("震幅", shakeModel.amplitude);
            shakeModel.frequency = EditorGUILayout.FloatField("震频", shakeModel.frequency);
            shakeModel.duration = EditorGUILayout.FloatField("震屏时长", shakeModel.duration);
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
                EditorGUILayout.LabelField(" -------- 被依赖弹体 --------", EditorStyles.boldLabel);
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