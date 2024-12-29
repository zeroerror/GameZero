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
                var targetAction_p = property.FindPropertyRelative("targetAction");
                targetAction_p.DrawProperty("目标行为");

                var targetActionType_p = property.FindPropertyRelative("targetActionType");
                targetActionType_p.DrawProperty("目标行为类型");
            }

            EditorGUI.EndProperty();
        }
    }
}