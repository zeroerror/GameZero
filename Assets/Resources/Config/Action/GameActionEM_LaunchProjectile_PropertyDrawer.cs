using System.Linq;
using GamePlay.Bussiness.Logic;
using GamePlay.Core;
using UnityEditor;
using UnityEngine;

namespace GamePlay.Config
{
    [CustomEditor(typeof(GameActionEM_LaunchProjectile))]
    public class GameActionEM_LaunchProjectile_PropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            var projectileId = property.FindPropertyRelative("projectileId");
            var launchOffset = property.FindPropertyRelative("launchOffset");
            var launchProjectileSO = property.FindPropertyRelative("launchProjectileSO");
            var barrageType = property.FindPropertyRelative("barrageType");
            var customLaunchOffsetEM = property.FindPropertyRelative("customLaunchOffsetEM");
            var spreadModelEM = property.FindPropertyRelative("spreadModelEM");

            EditorGUI.PropertyField(position, launchProjectileSO, new GUIContent("投射物模板"));
            var launchProjectileSOObj = launchProjectileSO.objectReferenceValue;
            var launchProjectileSOType = launchProjectileSOObj == null ? 0 : ((GameProjectileSO)launchProjectileSOObj).typeId;
            projectileId.intValue = launchProjectileSOType;

            position.y += EditorGUIUtility.singleLineHeight;
            EditorGUI.PropertyField(position, launchOffset, new GUIContent("发射偏移"));

            position.y += EditorGUIUtility.singleLineHeight;
            EditorGUI.PropertyField(position, barrageType, new GUIContent("弹幕类型"));
            var barrageTypeValue = (GameProjectileBarrageType)barrageType.enumValueIndex;

            position.y += EditorGUIUtility.singleLineHeight;
            switch (barrageTypeValue)
            {
                case GameProjectileBarrageType.CustomLaunchOffset:
                    EditorGUI.PropertyField(position, customLaunchOffsetEM, new GUIContent("自定义发射偏移模型"));
                    break;
                case GameProjectileBarrageType.Spread:
                    EditorGUI.PropertyField(position, spreadModelEM, new GUIContent("散射模型"));
                    break;
            }

            EditorGUI.EndProperty();
        }
    }
}
