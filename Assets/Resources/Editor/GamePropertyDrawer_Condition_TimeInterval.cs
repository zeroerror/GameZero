using UnityEditor;
using UnityEngine;

namespace GamePlay.Config
{
    [CustomPropertyDrawer(typeof(GameBuffConditionEM_TimeInterval))]
    public class GamePropertyDrawer_ConditionTimeInterval : GamePropertyDrawer
    {
        protected override void _OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var isEnable_p = property.FindPropertyRelative("isEnable");
            var labelTxt = "条件 - 时间间隔";
            var isEnable = isEnable_p.DrawProperty_Bool(labelTxt);
            EditorGUI.indentLevel += 1;
            if (isEnable)
            {
                var timeInterval_p = property.FindPropertyRelative("timeInterval");
                timeInterval_p.DrawProperty_Float("时间间隔(s)");
            }
            EditorGUI.indentLevel -= 1;
        }
    }
}