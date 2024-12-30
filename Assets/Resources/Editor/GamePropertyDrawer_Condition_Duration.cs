using UnityEditor;
using UnityEngine;

namespace GamePlay.Config
{
    [CustomPropertyDrawer(typeof(GameBuffConditionEM_Duration))]
    public class GamePropertyDrawer_Condition_Duration : GamePropertyDrawer
    {
        protected override void _OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var isEnable_p = property.FindPropertyRelative("isEnable");
            var labelTxt = "条件 - 持续时间";
            var isEnable = isEnable_p.DrawProperty_Bool(labelTxt);

            EditorGUI.indentLevel += 1;
            if (isEnable)
            {
                var duration_p = property.FindPropertyRelative("duration");
                duration_p.DrawProperty_Float("持续时间");
            }
            EditorGUI.indentLevel -= 1;
        }
    }
}