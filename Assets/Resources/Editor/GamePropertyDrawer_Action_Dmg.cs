using GamePlay.Bussiness.Logic;
using GamePlay.Bussiness.Renderer;
using UnityEditor;
using UnityEngine;

namespace GamePlay.Config
{
    [CustomPropertyDrawer(typeof(GameActionEM_Dmg))]
    public class GamePropertyDrawer_ActionDmg : GamePropertyDrawer
    {
        protected override void _OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var dmgType_p = property.FindPropertyRelative("dmgType");
            dmgType_p.DrawProperty_EnumPopup<GameActionDmgType>("伤害类型");

            var value_p = property.FindPropertyRelative("value");
            value_p.DrawProperty_Int("伤害数值");

            var randomValueOffset_p = property.FindPropertyRelative("randomValueOffset");
            randomValueOffset_p.DrawProperty_Vector2("随机值偏移");

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
        }
    }
}