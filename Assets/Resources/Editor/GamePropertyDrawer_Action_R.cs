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

            var vfxPrefab_p = property.FindPropertyRelative("vfxPrefab");
            vfxPrefab_p.DrawProperty("特效预制体");
            var vfxScale_p = property.FindPropertyRelative("vfxScale");
            vfxScale_p.DrawProperty("特效缩放");
            var vfxOffset_p = property.FindPropertyRelative("vfxOffset");
            vfxOffset_p.DrawProperty("特效偏移");

            var camShakeModel_p = property.FindPropertyRelative("camShakeModel");
            camShakeModel_p.DrawProperty("相机震动模型");

            EditorGUI.EndProperty();
        }
    }
}