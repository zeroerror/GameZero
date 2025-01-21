using UnityEditor;
using UnityEngine;

namespace GamePlay.Config
{
    [CustomPropertyDrawer(typeof(GameBuffConditionEM_WhenAttributeChange))]
    public class GamePropertyDrawer_Condition_WhenAttributeChange : GamePropertyDrawer
    {
        protected override void _OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var isEnable_p = property.FindPropertyRelative("isEnable");
            var labelTxt = "条件 - 当属性变化时";
            var isEnable = isEnable_p.DrawProperty_Bool(labelTxt);

            EditorGUI.indentLevel += 1;
            if (isEnable)
            {
                var valueA_p = property.FindPropertyRelative("valueA");
                valueA_p.DrawProperty_Int("属性A");
                var valueFormatA_p = property.FindPropertyRelative("valueFormatA");
                valueFormatA_p.DrawProperty("属性A格式");
                var refTypeA_p = property.FindPropertyRelative("refTypeA");
                refTypeA_p.DrawProperty("属性A参考类型");

                var valueB_p = property.FindPropertyRelative("valueB");
                valueB_p.DrawProperty_Int("属性B");
                var valueFormatB_p = property.FindPropertyRelative("valueFormatB");
                valueFormatB_p.DrawProperty("属性B格式");
                var refTypeB_p = property.FindPropertyRelative("refTypeB");
                refTypeB_p.DrawProperty("属性B参考类型");

                var compareType_p = property.FindPropertyRelative("compareType");
                compareType_p.DrawProperty("比较类型");
            }
            EditorGUI.indentLevel -= 1;
        }

    }
}