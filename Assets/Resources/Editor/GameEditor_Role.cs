using GamePlay.Bussiness.Logic;
using GamePlay.Core;
using UnityEditor;
using UnityEngine;

namespace GamePlay.Config
{
    [CustomEditor(typeof(GameRoleSO))]
    public class GameEditor_Role : Editor
    {
        private SerializedObject _serializedObject;
        private SerializedProperty typeId_p;
        private SerializedProperty roleName_p;
        private SerializedProperty desc_p;
        private SerializedProperty prefab_p;
        private SerializedProperty skills_p;
        private SerializedProperty attributes_p;
        private SerializedProperty prefabUrl_p;
        private SerializedProperty careerType_p;
        private SerializedProperty isMultyAnimationLayer_p;

        private void OnEnable()
        {
            this._serializedObject = new SerializedObject(target);
            this.typeId_p = _serializedObject.FindProperty("typeId");
            this.careerType_p = _serializedObject.FindProperty("careerType");
            this.roleName_p = _serializedObject.FindProperty("roleName");
            this.desc_p = _serializedObject.FindProperty("desc");
            this.prefab_p = _serializedObject.FindProperty("prefab");
            this.skills_p = _serializedObject.FindProperty("skills");
            this.attributes_p = _serializedObject.FindProperty("attributes");
            this.prefabUrl_p = _serializedObject.FindProperty("prefabUrl");
            this.isMultyAnimationLayer_p = _serializedObject.FindProperty("isMultyAnimationLayer");
        }

        public override void OnInspectorGUI()
        {
            this._serializedObject.Update();

            this.typeId_p.DrawProperty_Int("类型ID");
            this.careerType_p.DrawProperty_EnumPopup<GameRoleCareerType>("职业类型");
            this.roleName_p.DrawProperty_Str("名称");
            this.desc_p.DrawProperty_Str("描述");
            var prefab = this.prefab_p.DrawProperty<GameObject>("预制体");
            this.skills_p.DrawProperty("技能列表");
            this.attributes_p.DrawProperty("属性列表");

            if (prefab)
            {
                this.prefabUrl_p.stringValue = prefab.GetPrefabUrl();
            }
            else
            {
                this.prefabUrl_p.stringValue = "";
            }

            this.isMultyAnimationLayer_p.DrawProperty_Bool("是否为多层级动画");
            this._serializedObject.ApplyModifiedProperties();
        }

    }
}