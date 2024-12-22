using GamePlay.Bussiness.Logic;
using UnityEditor;
using UnityEngine;
namespace GamePlay.Config
{
    [CustomPropertyDrawer(typeof(GameActionPreconditionSetEM))]
    public class GamePropertyDrawer_Action_Precondition : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            var buffConditionEM_p = property.FindPropertyRelative("buffConditionEM");
            buffConditionEM_p.DrawProperty();

            EditorGUI.EndProperty();
        }
    }
}

