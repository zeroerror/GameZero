using UnityEditor;
using UnityEngine;

namespace GamePlay.Config
{
    [CustomPropertyDrawer(typeof(GameActionEMR))]
    public class GamePropertyDrawer_Action_R : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            EditorGUILayout.LabelField("视觉效果等表现参数", EditorStyles.boldLabel);

            property.FindPropertyRelative("actEffectPrefab").DrawProperty("行为特效");
            property.FindPropertyRelative("actVFXScale").DrawProperty("行为特效缩放");
            property.FindPropertyRelative("actVFXOffset").DrawProperty("行为特效偏移");
            property.FindPropertyRelative("actCamShakeModel").DrawProperty("行为相机震动");

            property.FindPropertyRelative("hitEffectPrefab").DrawProperty("命中特效");
            property.FindPropertyRelative("hitVFXScale").DrawProperty("命中特效缩放");
            property.FindPropertyRelative("hitVFXOffset").DrawProperty("命中特效偏移");
            property.FindPropertyRelative("hitCamShakeModel").DrawProperty("命中相机震动");

            EditorGUI.EndProperty();
        }
    }
}