using GamePlay.Bussiness.Logic;
using UnityEditor;
using UnityEngine;

namespace GamePlay.Config
{
    [CustomPropertyDrawer(typeof(GameProjectileStateEM))]
    public class GamePropertyDrawer_ProjectileState : GamePropertyDrawer
    {
        protected override void _OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // 状态模型
            var stateType_p = property.FindPropertyRelative("stateType");
            var stateType = stateType_p.DrawProperty_EnumPopup<GameProjectileStateType>("状态类型");
            if (stateType == GameProjectileStateType.None) EditorGUILayout.HelpBox("请选择状态类型", MessageType.Warning);
            switch (stateType)
            {
                case GameProjectileStateType.Attach:
                    property.FindPropertyRelative("attachStateEM").DrawProperty("状态参数");
                    break;
                case GameProjectileStateType.FixedDirection:
                    EditorGUILayout.LabelField("状态参数");
                    var fixedDirectionStateEM_p = property.FindPropertyRelative("fixedDirectionStateEM");
                    var speed_p = fixedDirectionStateEM_p.FindPropertyRelative("speed");
                    speed_p.DrawProperty_Float("速度");
                    var bounceCount_p = fixedDirectionStateEM_p.FindPropertyRelative("bounceCount");
                    var bounceCount = bounceCount_p.DrawProperty_Int("反弹次数");
                    if (bounceCount > 0)
                    {
                        var detectSelector_p = fixedDirectionStateEM_p.FindPropertyRelative("detectSelector");
                        detectSelector_p.DrawProperty("用于检测的实体选择器");
                    }
                    break;
                case GameProjectileStateType.LockOnEntity:
                    property.FindPropertyRelative("lockOnEntityStateEM").DrawProperty("状态参数");
                    break;
                case GameProjectileStateType.LockOnPosition:
                    property.FindPropertyRelative("lockOnPositionStateEM").DrawProperty("状态参数");
                    break;
                default:
                    break;
            }

            // 触发器集合
            this._DrawTriggerModel(property, "durationTriggerModel", "触发器[持续时间]");
            this._DrawTriggerModel(property, "volumeCollisionTriggerModel", "触发器[体积碰撞]");
            if (stateType == GameProjectileStateType.LockOnEntity || stateType == GameProjectileStateType.LockOnPosition) this._DrawTriggerModel(property, "impactTargetTriggerModel", "触发器[抵达目标]");
            else this._DisableTriggerModel(property, "impactTargetTriggerModel");
        }

        private void _DrawTriggerModel(SerializedProperty property, string triggerName, string label)
        {
            var color = GUI.color;

            var emSet_p = property.FindPropertyRelative("emSet");
            var trigger_p = emSet_p.FindPropertyRelative(triggerName);
            if (trigger_p == null) trigger_p = new SerializedObject(emSet_p.objectReferenceValue).FindProperty(triggerName);
            var triggerEnable_p = trigger_p.FindPropertyRelative("enable");
            var triggerEnable = triggerEnable_p.boolValue;
            if (triggerEnable)
            {
                // 浅蓝色
                GUI.color = new Color(122 / 255f, 169 / 255f, 238 / 255f);
            }
            triggerEnable_p.DrawProperty_Bool($"{label}");
            if (triggerEnable) trigger_p.DrawProperty();

            GUI.color = color;
        }

        private void _DisableTriggerModel(SerializedProperty property, string triggerName)
        {
            var emSet_p = property.FindPropertyRelative("emSet");
            var trigger_p = emSet_p.FindPropertyRelative(triggerName);
            var triggerEnable_p = trigger_p.FindPropertyRelative("enable");
            triggerEnable_p.boolValue = false;
        }
    }
}