using GamePlay.Bussiness.Logic;
using UnityEditor;
using UnityEngine;

namespace GamePlay.Config
{
    [CustomPropertyDrawer(typeof(GameActionEM_LaunchProjectile))]
    public class GamePropertyDrawer_LaunchProjectileAction : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            var launchProjectileSO_p = property.FindPropertyRelative("launchProjectileSO");
            var launchProjectileSO = launchProjectileSO_p.DrawProperty<GameProjectileSO>("投射物模板");

            var projectileId_p = property.FindPropertyRelative("projectileId");
            projectileId_p.intValue = launchProjectileSO ? launchProjectileSO.typeId : 0;

            // 绘制 "发射偏移"
            var launchOffset_p = property.FindPropertyRelative("launchOffset");
            launchOffset_p.DrawProperty_Vector2("发射偏移");

            // 绘制 "弹幕类型"
            var barrageType_p = property.FindPropertyRelative("barrageType");
            var barrageTypeValue = barrageType_p.DrawProperty_EnumPopup<GameProjectileBarrageType>("弹幕类型");

            // 根据弹幕类型显示不同字段
            switch (barrageTypeValue)
            {
                case GameProjectileBarrageType.CustomLaunchOffset:
                    var customLaunchOffsetEM_p = property.FindPropertyRelative("customLaunchOffsetEM");
                    EditorGUILayout.PropertyField(customLaunchOffsetEM_p, new GUIContent("自定义发射偏移模型"), true);
                    break;
                case GameProjectileBarrageType.Spread:
                    var spreadModelEM_p = property.FindPropertyRelative("spreadEM");
                    EditorGUILayout.PropertyField(spreadModelEM_p, new GUIContent("散射模型"), true);
                    break;
            }

            EditorGUI.EndProperty();

            if (property.serializedObject.hasModifiedProperties) property.serializedObject.ApplyModifiedProperties();
        }
    }
}
