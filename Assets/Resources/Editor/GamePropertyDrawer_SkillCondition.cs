using GamePlay.Bussiness.Logic;
using UnityEditor;
using UnityEngine;

namespace GamePlay.Config
{
    [CustomPropertyDrawer(typeof(GameSkillConditionEM))]
    public class GamePropertyDrawer_SKillCondition : GamePropertyDrawer
    {
        protected override void _OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUILayout.LabelField("表现层参数", EditorStyles.boldLabel);
            property.FindPropertyRelative("targeterType").DrawProperty_EnumPopup<GameSkillTargterType>("技能目标类型");
            property.FindPropertyRelative("cdTime").DrawProperty_Float("冷却时间(s)");
            property.FindPropertyRelative("mpCost").DrawProperty_Float("消耗MP");
            property.FindPropertyRelative("selectorEM").DrawProperty();
        }
    }
}