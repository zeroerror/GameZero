using GamePlay.Bussiness.Config;
using GamePlay.Bussiness.Logic;
using UnityEditor;
using UnityEngine;

namespace GamePlay.Config
{
    [CustomPropertyDrawer(typeof(GameActionValueRefEM))]
    public class GamePropertyDrawer_Action_ValueRef : GamePropertyDrawer
    {
        protected override void _OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var value_p = property.FindPropertyRelative("value");
            value_p.DrawProperty_Int("数值");

            var valueFormat_p = property.FindPropertyRelative("valueFormat");
            valueFormat_p.DrawProperty_EnumPopup<GameActionValueFormat>("数值格式");

            var refType_p = property.FindPropertyRelative("refType");
            refType_p.DrawProperty_EnumPopup<GameActionValueRefType>("参考类型");
        }
    }
}