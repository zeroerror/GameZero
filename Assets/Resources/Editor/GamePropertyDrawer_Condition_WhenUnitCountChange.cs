using UnityEditor;
using UnityEngine;

namespace GamePlay.Config
{
    [CustomPropertyDrawer(typeof(GameBuffConditionEM_WhenUnitCountChange))]
    public class GamePropertyDrawer_Condition_WhenUnitCountChange : GamePropertyDrawer
    {
        protected override void _OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var isEnable_p = property.FindPropertyRelative("isEnable");
            var labelTxt = "条件 - 当单位数量变化时";
            var isEnable = isEnable_p.DrawProperty_Bool(labelTxt);

            EditorGUI.indentLevel += 1;
            if (isEnable)
            {
                var selectorEM_p = property.FindPropertyRelative("selectorEM");
                selectorEM_p.DrawProperty("单位筛选器");
                var countCondition_p = property.FindPropertyRelative("countCondition");
                countCondition_p.DrawProperty("单位数量条件");
            }
            EditorGUI.indentLevel -= 1;
        }

    }
}