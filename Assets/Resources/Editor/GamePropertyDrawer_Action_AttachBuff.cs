using UnityEditor;
using UnityEngine;

namespace GamePlay.Config
{
    [CustomPropertyDrawer(typeof(GameActionEM_AttachBuff))]
    public class GamePropertyDrawer_Action_AttachBuff : GamePropertyDrawer
    {
        protected override void _OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var buffSO_P = property.FindPropertyRelative("buffSO");
            buffSO_P.DrawProperty("buffSO");

            var layer = property.FindPropertyRelative("layer");
            layer.DrawProperty_Int("层数(至少一层)");
            if (layer.intValue < 1)
            {
                layer.intValue = 1;
            }

            var selectorEM_p = property.FindPropertyRelative("selectorEM");
            selectorEM_p.DrawProperty();

            var preconditionSetEM_p = property.FindPropertyRelative("preconditionSetEM");
            preconditionSetEM_p.DrawProperty();
        }
    }
}