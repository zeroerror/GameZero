using UnityEditor;
using UnityEngine;
namespace GamePlay.Config
{
    [CustomPropertyDrawer(typeof(GameActionPreconditionEM_Probability))]
    public class GamePropertyDrawer_Action_Precondition_Probability : GamePropertyDrawer
    {
        protected override void _OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUILayout.LabelField("前置条件 - 概率");
            var enable_p = property.FindPropertyRelative("enable");
            var enable = enable_p.DrawProperty_Bool("是否启用");
            if (enable)
            {
                var probability_p = property.FindPropertyRelative("probability");
                probability_p.DrawProperty_Float("概率");
            }
        }

    }
}

