using System.Runtime.InteropServices;
using GamePlay.Bussiness.Logic;
using UnityEditor;
using UnityEngine;

namespace GamePlay.Config
{
    [CustomPropertyDrawer(typeof(GameActionEM_SummonRoles))]
    public class GamePropertyDrawer_Action_SummonRoles : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            var roleId_P = property.FindPropertyRelative("roleId");
            roleId_P.DrawProperty("角色ID");

            var count_p = property.FindPropertyRelative("count");
            count_p.DrawProperty_Int("个数(至少一个)");
            if (count_p.intValue < 1)
            {
                count_p.intValue = 1;
            }

            var campType_p = property.FindPropertyRelative("campType");
            campType_p.DrawProperty_EnumPopup<GameCampType>("阵营");

            var selectorEM_p = property.FindPropertyRelative("selectorEM");
            selectorEM_p.DrawProperty();

            var preconditionSetEM_p = property.FindPropertyRelative("preconditionSetEM");
            preconditionSetEM_p.DrawProperty();

            EditorGUI.EndProperty();
        }
    }
}