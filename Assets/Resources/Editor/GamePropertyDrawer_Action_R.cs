using UnityEditor;
using UnityEngine;

namespace GamePlay.Config
{
    [CustomPropertyDrawer(typeof(GameActionEMR))]
    public class GamePropertyDrawer_Action_R : GamePropertyDrawer
    {
        protected override void _OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUILayout.LabelField("表现层参数", EditorStyles.boldLabel);

            var desc_p = property.FindPropertyRelative("desc");
            desc_p.DrawProperty_Str("描述");
            var actEffectPrefab_p = property.FindPropertyRelative("actEffectPrefab");
            actEffectPrefab_p.DrawProperty("行为特效");
            var actVFXScale_p = property.FindPropertyRelative("actVFXScale");
            actVFXScale_p.DrawProperty_Vector2("行为特效缩放");
            var actVFXOffset_p = property.FindPropertyRelative("actVFXOffset");
            actVFXOffset_p.DrawProperty_Vector2("行为特效偏移");
            var actCamShakeModel_p = property.FindPropertyRelative("actCamShakeModel");
            actCamShakeModel_p.DrawProperty("行为相机震动");

            var hitEffectPrefab_p = property.FindPropertyRelative("hitEffectPrefab");
            hitEffectPrefab_p.DrawProperty("命中特效");
            var hitVFXScale_p = property.FindPropertyRelative("hitVFXScale");
            hitVFXScale_p.DrawProperty_Vector2("命中特效缩放");
            var hitVFXOffset_p = property.FindPropertyRelative("hitVFXOffset");
            hitVFXOffset_p.DrawProperty_Vector2("命中特效偏移");
            var hitCamShakeModel_p = property.FindPropertyRelative("hitCamShakeModel");
            hitCamShakeModel_p.DrawProperty("命中相机震动");
        }
    }
}