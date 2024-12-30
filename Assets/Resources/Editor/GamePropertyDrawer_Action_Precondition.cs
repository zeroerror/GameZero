using GamePlay.Bussiness.Logic;
using UnityEditor;
using UnityEngine;
namespace GamePlay.Config
{
    [CustomPropertyDrawer(typeof(GameActionPreconditionSetEM))]
    public class GamePropertyDrawer_Action_Precondition : GamePropertyDrawer
    {
        protected override void _OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var buffConditionEM_p = property.FindPropertyRelative("buffConditionEM");
            buffConditionEM_p.DrawProperty();
        }
    }
}

