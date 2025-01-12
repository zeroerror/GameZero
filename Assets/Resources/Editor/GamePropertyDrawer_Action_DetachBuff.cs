using UnityEditor;
using UnityEngine;

namespace GamePlay.Config
{
    [CustomPropertyDrawer(typeof(GameActionEM_DetachBuff))]
    public class GamePropertyDrawer_Action_DetachBuff : GamePropertyDrawer
    {
        protected override void _OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var buffSO_P = property.FindPropertyRelative("buffSO");
            buffSO_P.DrawProperty("buffSO");

            var layer = property.FindPropertyRelative("layer");
            layer.DrawProperty_Int("层数(0代表移除全部)");
            if (layer.intValue < 1)
            {
                layer.intValue = 1;
            }

            // var randomValueOffset_p = property.FindPropertyRelative("randomValueOffset");
            // randomValueOffset_p.DrawProperty_Vector2("随机值偏移");

            var selectorEM_p = property.FindPropertyRelative("selectorEM");
            selectorEM_p.DrawProperty();

            var preconditionSetEM_p = property.FindPropertyRelative("preconditionSetEM");
            preconditionSetEM_p.DrawProperty();
        }
    }
}