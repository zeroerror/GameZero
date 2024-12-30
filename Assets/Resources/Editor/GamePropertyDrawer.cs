using UnityEditor;
using UnityEngine;
namespace GamePlay.Config
{
    public abstract class GamePropertyDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return 0;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            _OnGUI(position, property, label);
            EditorGUI.EndProperty();
        }

        protected abstract void _OnGUI(Rect position, SerializedProperty property, GUIContent label);
    }
}