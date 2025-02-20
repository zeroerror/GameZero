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

            var actSFX_p = property.FindPropertyRelative("actSFX");
            actSFX_p.DrawProperty("行为音效");
            var actSFXVolume_p = property.FindPropertyRelative("actSFXVolume");
            actSFXVolume_p.DrawProperty("行为音效音量");
            var actVFX_p = property.FindPropertyRelative("actVFX");
            actVFX_p.DrawProperty("行为视觉特效");
            var actVFXScale_p = property.FindPropertyRelative("actVFXScale");
            actVFXScale_p.DrawProperty_Vector2("行为特效缩放");
            var actVFXOffset_p = property.FindPropertyRelative("actVFXOffset");
            actVFXOffset_p.DrawProperty_Vector2("行为特效偏移");
            var actCamShakeModel_p = property.FindPropertyRelative("actCamShakeModel");
            actCamShakeModel_p.DrawProperty("行为相机震动");

            var hitSFX_p = property.FindPropertyRelative("hitSFX");
            hitSFX_p.DrawProperty("命中音效");
            var hitSFXVolume_p = property.FindPropertyRelative("hitSFXVolume");
            hitSFXVolume_p.DrawProperty("命中音效音量");
            var hitVFX_p = property.FindPropertyRelative("hitVFX");
            hitVFX_p.DrawProperty("命中视觉特效");
            var hitVFXScale_p = property.FindPropertyRelative("hitVFXScale");
            hitVFXScale_p.DrawProperty_Vector2("命中特效缩放");
            var hitVFXOffset_p = property.FindPropertyRelative("hitVFXOffset");
            hitVFXOffset_p.DrawProperty_Vector2("命中特效偏移");
            var hitCamShakeModel_p = property.FindPropertyRelative("hitCamShakeModel");
            hitCamShakeModel_p.DrawProperty("命中相机震动");
        }
    }
}