using System.Linq;
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
        private SerializedProperty roleName_p;
        private SerializedProperty desc_p;
        private SerializedProperty prefab_p;
        private SerializedProperty skills_p;
        private SerializedProperty attributes_p;
        private SerializedProperty _prefabUrl_p;

        private void OnEnable()
        {
            this._serializedObject = new SerializedObject(target);
            this.roleName_p = _serializedObject.FindProperty("roleName");
            this.desc_p = _serializedObject.FindProperty("desc");
            this.prefab_p = _serializedObject.FindProperty("prefab");
            this.skills_p = _serializedObject.FindProperty("skills");
            this.attributes_p = _serializedObject.FindProperty("attributes");
            this._prefabUrl_p = _serializedObject.FindProperty("_prefabUrl");
        }

        public override void OnInspectorGUI()
        {
            this._serializedObject.Update();

            this.roleName_p.DrawProperty_Str("名称");
            this.desc_p.DrawProperty_Str("描述");
            var prefab = this.prefab_p.DrawProperty<GameObject>("预制体");
            this.skills_p.DrawProperty("技能列表");
            this.attributes_p.DrawProperty("属性列表");

            if (prefab)
            {
                this._prefabUrl_p.stringValue = AssetDatabase.GetAssetPath(prefab);
            }
            else
            {
                this._prefabUrl_p.stringValue = "";
            }

            this._serializedObject.ApplyModifiedProperties();
        }

    }
}