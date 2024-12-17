using UnityEditor;
using UnityEngine;

namespace GamePlay.Config
{
    [CustomPropertyDrawer(typeof(GameBuffConditionEM_Duration))]
    public class GamePropertyDrawer_ConditionDuration : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            var isEnable_p = property.FindPropertyRelative("isEnable");
            var isEnable = isEnable_p.DrawProperty_Bool("是否启用");

            if (isEnable)
            {
                var duration_p = property.FindPropertyRelative("duration");
                duration_p.DrawProperty_Float("持续时间");
            }

            EditorGUI.EndProperty();
        }
    }
}