using GamePlay.Bussiness.Logic;
using GamePlay.Bussiness.Renderer;
using UnityEditor;
using UnityEngine;

namespace GamePlay.Config
{
    [CustomPropertyDrawer(typeof(GameActionEM_KnockBack))]
    public class GamePropertyDrawer_KnockBackActionEM : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            var knockBackDirType_P = property.FindPropertyRelative("knockBackDirType");
            knockBackDirType_P.DrawProperty_EnumPopup<GameActionKnockBackDirType>("击退方向类型");

            var distance_p = property.FindPropertyRelative("distance");
            distance_p.DrawProperty_Float("击退距离");

            var duration_p = property.FindPropertyRelative("duration");
            duration_p.DrawProperty_Float("持续时间");

            var easingType_p = property.FindPropertyRelative("easingType");
            easingType_p.DrawProperty_EnumPopup<GameEasingType>("缓动曲线");


            var selectorEM_p = property.FindPropertyRelative("selectorEM");
            selectorEM_p.DrawProperty();

            EditorGUI.EndProperty();
        }
    }
}