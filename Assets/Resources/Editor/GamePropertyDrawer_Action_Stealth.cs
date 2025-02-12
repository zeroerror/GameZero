using UnityEditor;
using UnityEngine;

namespace GamePlay.Config
{
    [CustomPropertyDrawer(typeof(GameActionEM_Stealth))]
    public class GamePropertyDrawer_Action_Stealth : GamePropertyDrawer
    {
        protected override void _OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var randomValueOffset_p = property.FindPropertyRelative("randomValueOffset");
            randomValueOffset_p.DrawProperty_Vector2("持续时间随机差值");

            var selectorEM_p = property.FindPropertyRelative("selectorEM");
            selectorEM_p.DrawProperty();

            var preconditionSetEM_p = property.FindPropertyRelative("preconditionSetEM");
            preconditionSetEM_p.DrawProperty();

            var duration_p = property.FindPropertyRelative("duration");
            duration_p.DrawProperty_Float("持续时间");
        }
    }
}