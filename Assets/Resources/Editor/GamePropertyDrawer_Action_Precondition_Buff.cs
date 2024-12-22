using UnityEditor;
using UnityEngine;
namespace GamePlay.Config
{
    [CustomPropertyDrawer(typeof(GameActionPreconditionEM_Buff))]
    public class GamePropertyDrawer_Action_Precondition_Buff : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            EditorGUILayout.LabelField("前置条件 - buff");
            var enable_p = property.FindPropertyRelative("enable");
            var enable = enable_p.DrawProperty_Bool("是否启用");
            if (enable)
            {
                var buffSO_p = property.FindPropertyRelative("buffSO");
                buffSO_p.DrawProperty("buffSO");
                var layer_p = property.FindPropertyRelative("layer");
                var layer = layer_p.DrawProperty_Int("所需层数(至少一层)");
                if (layer < 1) layer_p.intValue = 1;
            }

            EditorGUI.EndProperty();
        }
    }
}

