using UnityEditor;
using UnityEngine;

namespace GamePlay.Config
{
    [CustomPropertyDrawer(typeof(GameBuffConditionEM_WhenDoAction))]
    public class GamePropertyDrawer_Condition_WhenDoAction : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            var isEnable_p = property.FindPropertyRelative("isEnable");
            var labelTxt = "条件 - 当执行行为时";
            var isEnable = isEnable_p.DrawProperty_Bool(labelTxt);

            if (isEnable)
            {
                var duration_p = property.FindPropertyRelative("targetAction");
                duration_p.DrawProperty("目标行为");
            }

            EditorGUI.EndProperty();
        }
    }
}