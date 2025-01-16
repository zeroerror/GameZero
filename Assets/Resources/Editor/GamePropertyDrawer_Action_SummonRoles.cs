using GamePlay.Bussiness.Logic;
using UnityEditor;
using UnityEngine;

namespace GamePlay.Config
{
    [CustomPropertyDrawer(typeof(GameActionEM_SummonRoles))]
    public class GamePropertyDrawer_Action_SummonRoles : GamePropertyDrawer
    {
        protected override void _OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var roleSO_P = property.FindPropertyRelative("roleSO");
            roleSO_P.DrawProperty("角色");

            var count_p = property.FindPropertyRelative("count");
            count_p.DrawProperty_Int("个数(至少一个)");
            if (count_p.intValue < 1)
            {
                count_p.intValue = 1;
            }
            var randomValueOffset_p = property.FindPropertyRelative("randomValueOffset");
            randomValueOffset_p.DrawProperty_Vector2("随机值偏移");

            var campType_p = property.FindPropertyRelative("campType");
            campType_p.DrawProperty_EnumPopup<GameCampType>("阵营");

            var selectorEM_p = property.FindPropertyRelative("selectorEM");
            selectorEM_p.DrawProperty();

            var preconditionSetEM_p = property.FindPropertyRelative("preconditionSetEM");
            preconditionSetEM_p.DrawProperty();
        }
    }
}