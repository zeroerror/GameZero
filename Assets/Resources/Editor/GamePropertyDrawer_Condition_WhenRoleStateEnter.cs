using UnityEditor;
using UnityEngine;

namespace GamePlay.Config
{
    [CustomPropertyDrawer(typeof(GameBuffConditionEM_WhenRoleStateEnter))]
    public class GamePropertyDrawer_Condition_WhenRoleStateEnter : GamePropertyDrawer
    {
        protected override void _OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var isEnable_p = property.FindPropertyRelative("isEnable");
            var labelTxt = "条件 - 当角色状态进入时";
            var isEnable = isEnable_p.DrawProperty_Bool(labelTxt);

            EditorGUI.indentLevel += 1;
            if (isEnable)
            {
                var campType_p = property.FindPropertyRelative("campType");
                campType_p.DrawProperty("筛选阵营类型(None代表自己)");
                var stateType_p = property.FindPropertyRelative("stateType");
                var stateType = stateType_p.DrawProperty_EnumPopup<GameRoleStateType>("角色进入的状态类型");
                if (stateType == GameRoleStateType.Cast)
                {
                    var skillType_p = property.FindPropertyRelative("skillType");
                    skillType_p.DrawProperty("技能类型(作为施法状态的进一步筛选)");
                }
            }
            EditorGUI.indentLevel -= 1;
        }

    }
}