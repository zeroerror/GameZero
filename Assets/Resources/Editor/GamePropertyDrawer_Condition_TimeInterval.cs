using UnityEditor;
using UnityEngine;

namespace GamePlay.Config
{
    [CustomPropertyDrawer(typeof(GameBuffConditionEM_TimeInterval))]
    public class GamePropertyDrawer_ConditionTimeInterval : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            var isEnable_p = property.FindPropertyRelative("isEnable");
            var labelTxt = "条件 - 时间间隔";
            var isEnable = isEnable_p.DrawProperty_Bool(labelTxt);

            if (isEnable)
            {
                var timeInterval_p = property.FindPropertyRelative("timeInterval");
                timeInterval_p.DrawProperty_Float("时间间隔(s)");
            }

            EditorGUI.EndProperty();
        }
    }
}