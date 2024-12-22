using GamePlay.Bussiness.Logic;
using UnityEditor;
using UnityEngine;

namespace GamePlay.Config
{
    [CustomPropertyDrawer(typeof(GameActionEM_Attribute))]
    public class GamePropertyDrawer_Action_Attribute : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            var modifyType_p = property.FindPropertyRelative("modifyType");
            modifyType_p.DrawProperty_EnumPopup<GameAttributeType>("属性修改类型");

            var value_p = property.FindPropertyRelative("value");
            value_p.DrawProperty_Int("属性修改数值");

            var valueFormat_p = property.FindPropertyRelative("valueFormat");
            var valueFormat = valueFormat_p.DrawProperty_EnumPopup<GameActionValueFormat>("数值格式");

            if (valueFormat == GameActionValueFormat.Fixed)
            {
                var refType_p = property.FindPropertyRelative("refType");
                refType_p.enumValueIndex = (int)GameActionValueRefType.Fixed;
            }
            else
            {
                var refType_p = property.FindPropertyRelative("refType");
                refType_p.DrawProperty_EnumPopup<GameActionValueRefType>("数值参考类型");
            }

            var selectorEM_p = property.FindPropertyRelative("selectorEM");
            selectorEM_p.DrawProperty();

            var preconditionSetEM_p = property.FindPropertyRelative("preconditionSetEM");
            preconditionSetEM_p.DrawProperty();

            EditorGUI.EndProperty();
        }
    }
}