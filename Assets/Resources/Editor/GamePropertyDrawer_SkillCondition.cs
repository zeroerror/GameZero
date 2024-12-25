using GamePlay.Bussiness.Logic;
using UnityEditor;
using UnityEngine;

namespace GamePlay.Config
{
    [CustomPropertyDrawer(typeof(GameSkillConditionEM))]
    public class GamePropertyDrawer_SKillCondition : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            EditorGUILayout.LabelField("视觉效果等表现参数", EditorStyles.boldLabel);

            property.FindPropertyRelative("targeterType").DrawProperty_EnumPopup<GameSkillTargterType>("技能目标类型");
            property.FindPropertyRelative("cdTime").DrawProperty_Float("冷却时间(s)");
            property.FindPropertyRelative("mpCost").DrawProperty_Float("消耗MP");
            property.FindPropertyRelative("selectorEM").DrawProperty();

            EditorGUI.EndProperty();
        }
    }
}