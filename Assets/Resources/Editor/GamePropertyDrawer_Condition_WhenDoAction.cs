using UnityEditor;
using UnityEngine;

namespace GamePlay.Config
{
    [CustomPropertyDrawer(typeof(GameBuffConditionEM_WhenDoAction))]
    public class GamePropertyDrawer_Condition_WhenDoAction : GamePropertyDrawer
    {
        protected override void _OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var isEnable_p = property.FindPropertyRelative("isEnable");
            var labelTxt = "条件 - 当执行行为时";
            var isEnable = isEnable_p.DrawProperty_Bool(labelTxt);

            EditorGUI.indentLevel += 1;
            if (isEnable)
            {
                var targetAction_p = property.FindPropertyRelative("targetAction");
                targetAction_p.DrawProperty("目标行为");

                var targetActionType_p = property.FindPropertyRelative("targetActionType");
                targetActionType_p.DrawProperty("目标行为类型");

                var actionCount_p = property.FindPropertyRelative("actionCount");
                var actionCount = actionCount_p.DrawProperty_Int("每N次执行");
                if (actionCount <= 0)
                {
                    actionCount_p.intValue = 1;
                }

                var validWindowTime_p = property.FindPropertyRelative("validWindowTime");
                validWindowTime_p.DrawProperty_Float("有效窗口时间(秒, 0代表无限制)");
            }
            EditorGUI.indentLevel -= 1;
        }

    }
}