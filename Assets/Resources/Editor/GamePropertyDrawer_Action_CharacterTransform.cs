using GamePlay.Bussiness.Logic;
using GamePlay.Core;
using UnityEditor;
using UnityEngine;

namespace GamePlay.Config
{
    [CustomPropertyDrawer(typeof(GameActionEM_CharacterTransform))]
    public class GamePropertyDrawer_Action_CharacterTransform : GamePropertyDrawer
    {
        protected override void _OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var randomValueOffset_p = property.FindPropertyRelative("randomValueOffset");
            randomValueOffset_p.DrawProperty_Vector2("随机值偏移");

            var selectorEM_p = property.FindPropertyRelative("selectorEM");
            selectorEM_p.DrawProperty();

            var preconditionSetEM_p = property.FindPropertyRelative("preconditionSetEM");
            preconditionSetEM_p.DrawProperty();

            var selectAnchorType_p = selectorEM_p.FindPropertyRelative("selectAnchorType");
            var selectAnchorType = (GameEntitySelectAnchorType)selectAnchorType_p.enumValueIndex;
            var selColliderType_p = selectorEM_p.FindPropertyRelative("selColliderType");
            var selColliderType = (GameColliderType)selColliderType_p.enumValueIndex;
            var isTransActor = selectAnchorType == GameEntitySelectAnchorType.Actor && selColliderType == GameColliderType.None;
            if (!isTransActor)
            {
                var roleSO_p = property.FindPropertyRelative("roleSO");
                roleSO_p.DrawProperty("变身角色");
            }

            var attributeList_p = property.FindPropertyRelative("attributeList");
            attributeList_p.DrawProperty("属性列表");
        }
    }
}