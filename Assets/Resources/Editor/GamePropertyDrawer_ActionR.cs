using GamePlay.Bussiness.Renderer;
using UnityEditor;
using UnityEngine;

namespace GamePlay.Config
{
    [CustomPropertyDrawer(typeof(GameActionEMR))]
    public class GamePropertyDrawer_ActionR : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            var prefab_p = property.FindPropertyRelative("prefab");
            prefab_p.DrawProperty("预制体");

            var scale_p = property.FindPropertyRelative("scale");
            scale_p.DrawProperty("缩放");

            var camShakeModel_p = property.FindPropertyRelative("camShakeModel");
            camShakeModel_p.DrawProperty("相机震动模型");
        }
    }
}