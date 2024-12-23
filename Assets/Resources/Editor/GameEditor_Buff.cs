using System.Linq;
using GamePlay.Bussiness.Logic;
using GamePlay.Core;
using UnityEditor;
using UnityEngine;

namespace GamePlay.Config
{
    [CustomEditor(typeof(GameBuffSO))]
    public class GameEditor_Buff : Editor
    {
        private SerializedObject _serializedObject;
        private SerializedProperty typeId_p;
        private SerializedProperty buffName_p;
        private SerializedProperty desc_p;
        private SerializedProperty refreshFlag_p;
        private SerializedProperty maxLayer_p;
        private SerializedProperty actionSOs_p;
        private SerializedProperty conditionSetEM_action_p;
        private SerializedProperty conditionSetEM_remove_p;
        private SerializedProperty vfxPrefab_p;
        private SerializedProperty _vfxPrefabUrl_p;
        private SerializedProperty vfxLayerType_p;

        private void OnEnable()
        {
            this._serializedObject = new SerializedObject(target);
            this.typeId_p = _serializedObject.FindProperty("typeId");
            this.buffName_p = _serializedObject.FindProperty("buffName");
            this.desc_p = _serializedObject.FindProperty("desc");
            this.refreshFlag_p = _serializedObject.FindProperty("refreshFlag");
            this.maxLayer_p = _serializedObject.FindProperty("maxLayer");
            this.actionSOs_p = _serializedObject.FindProperty("actionSOs");
            this.conditionSetEM_action_p = _serializedObject.FindProperty("conditionSetEM_action");
            this.conditionSetEM_remove_p = _serializedObject.FindProperty("conditionSetEM_remove");
            this.vfxPrefab_p = _serializedObject.FindProperty("vfxPrefab");
            this._vfxPrefabUrl_p = _serializedObject.FindProperty("_vfxPrefabUrl");
            this.vfxLayerType_p = _serializedObject.FindProperty("vfxLayerType");
        }

        public override void OnInspectorGUI()
        {
            this._serializedObject.Update();

            this.typeId_p.DrawProperty_Int("类型ID");
            this.buffName_p.DrawProperty_Str("名称");
            this.desc_p.DrawProperty_Str("描述");
            this.refreshFlag_p.DrawProperty_EnumPopup<GameBuffRefreshFlag>("刷新类型标记");
            this.maxLayer_p.DrawProperty_Int("最大层数");
            this.actionSOs_p.DrawProperty("行为模板列表");
            this.conditionSetEM_action_p.DrawProperty("条件集模板 - 触发行为");
            this.conditionSetEM_remove_p.DrawProperty("条件集模板 - 移除");
            var vfxPrefab = this.vfxPrefab_p.DrawProperty<GameObject>("buff特效");
            if (vfxPrefab)
            {
                this._vfxPrefabUrl_p.stringValue = AssetDatabase.GetAssetPath(vfxPrefab);
            }
            else
            {
                this._vfxPrefabUrl_p.stringValue = "";
            }
            this.vfxLayerType_p.DrawProperty_EnumPopup<GameFieldLayerType>("挂载层级");

            this._serializedObject.ApplyModifiedProperties();
        }

    }
}