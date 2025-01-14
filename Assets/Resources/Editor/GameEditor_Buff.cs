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
        private SerializedProperty actionParam_p;
        private SerializedProperty actionSOs_p;
        private SerializedProperty actionCD_p;
        private SerializedProperty conditionSetEM_action_p;
        private SerializedProperty conditionSetEM_remove_p;
        private SerializedProperty layerSelectorEnable_p;
        private SerializedProperty layerSelectorEM_p;

        private SerializedProperty vfxPrefab_p;
        private SerializedProperty vfxPrefabUrl_p;
        private SerializedProperty vfxLayerType_p;
        private SerializedProperty vfxOrderOffset_p;
        private SerializedProperty vfxScale_p;
        private SerializedProperty vfxOffset_p;

        private SerializedProperty attributeEMs_p;

        private void OnEnable()
        {
            this._serializedObject = new SerializedObject(target);
            this.typeId_p = _serializedObject.FindProperty("typeId");
            this.buffName_p = _serializedObject.FindProperty("buffName");
            this.desc_p = _serializedObject.FindProperty("desc");
            this.refreshFlag_p = _serializedObject.FindProperty("refreshFlag");
            this.maxLayer_p = _serializedObject.FindProperty("maxLayer");
            this.actionParam_p = _serializedObject.FindProperty("actionParam");
            this.actionSOs_p = _serializedObject.FindProperty("actionSOs");
            this.actionCD_p = _serializedObject.FindProperty("actionCD");
            this.conditionSetEM_action_p = _serializedObject.FindProperty("conditionSetEM_action");
            this.conditionSetEM_remove_p = _serializedObject.FindProperty("conditionSetEM_remove");
            this.layerSelectorEnable_p = _serializedObject.FindProperty("layerSelectorEnable");
            this.layerSelectorEM_p = _serializedObject.FindProperty("layerSelectorEM");

            this.vfxPrefab_p = _serializedObject.FindProperty("vfxPrefab");
            this.vfxPrefabUrl_p = _serializedObject.FindProperty("vfxPrefabUrl");
            this.vfxLayerType_p = _serializedObject.FindProperty("vfxLayerType");
            this.vfxOrderOffset_p = _serializedObject.FindProperty("vfxOrderOffset");
            this.vfxScale_p = _serializedObject.FindProperty("vfxScale");
            this.vfxOffset_p = _serializedObject.FindProperty("vfxOffset");

            this.attributeEMs_p = _serializedObject.FindProperty("attributeEMs");
        }

        public override void OnInspectorGUI()
        {
            this._serializedObject.Update();

            GameEditorGUILayout.DrawBoxItem(() =>
            {
                this.typeId_p.DrawProperty_Int("类型ID");
                this.buffName_p.DrawProperty_Str("名称");
                this.desc_p.DrawProperty_Str("描述");
                var refreshFlag = this.refreshFlag_p.DrawProperty_EnumFlagsPopup<GameBuffRefreshFlag>("刷新类型标记");
                var maxLayer = this.maxLayer_p.DrawProperty_Int("最大层数");
                if (maxLayer < 1)
                {
                    this.maxLayer_p.intValue = 1;
                }

                // 对刷新类型标记的合法性进行校正
                if (refreshFlag.HasFlag(GameBuffRefreshFlag.StackLayer) && maxLayer < 2)
                {
                    refreshFlag &= ~GameBuffRefreshFlag.StackLayer;
                    this.refreshFlag_p.intValue = (int)refreshFlag;
                }
                else if (maxLayer > 1 && !refreshFlag.HasFlag(GameBuffRefreshFlag.StackLayer))
                {
                    refreshFlag |= GameBuffRefreshFlag.StackLayer;
                    this.refreshFlag_p.intValue = (int)refreshFlag;
                }

                this.actionParam_p.DrawProperty_Int("行为参数(百分比)");

                var vfxPrefab = this.vfxPrefab_p.DrawProperty<GameObject>("buff特效");
                if (vfxPrefab)
                {
                    this.vfxPrefabUrl_p.stringValue = vfxPrefab.GetPrefabUrl();

                    var vfxLayerType = this.vfxLayerType_p.DrawProperty_EnumPopup<GameFieldLayerType>("挂载层级");
                    if (vfxLayerType == GameFieldLayerType.None)
                    {
                        vfxLayerType = GameFieldLayerType.VFX;
                        this.vfxLayerType_p.enumValueIndex = (int)vfxLayerType;
                    }

                    this.vfxOrderOffset_p.DrawProperty_Int("层级偏移");

                    var vfxScale = this.vfxScale_p.DrawProperty_Vector2("缩放");
                    if (vfxScale.x <= 0 || vfxScale.y <= 0)
                    {
                        vfxScale = Vector2.one;
                        this.vfxScale_p.vector2Value = vfxScale;
                        EditorGUILayout.HelpBox("缩放参数错误, 使用默认值(1,1)", MessageType.Warning);
                    }

                    this.vfxOffset_p.DrawProperty_Vector2("坐标偏移");
                }
                else
                {
                    this.vfxPrefabUrl_p.stringValue = "";
                }
            });

            GameEditorGUILayout.DrawBoxItem(() =>
            {
                this.attributeEMs_p.DrawProperty_Array("属性效果");
            });

            GameEditorGUILayout.DrawBoxItem(() =>
            {
                this.actionSOs_p.DrawProperty("行为模板列表");
                this.actionCD_p.DrawProperty_Float("触发行为后的冷却时间");
            });
            GameEditorGUILayout.DrawBoxItem(() =>
            {
                this.conditionSetEM_action_p.DrawProperty("条件集模板 - 触发行为");
            });
            GameEditorGUILayout.DrawBoxItem(() =>
            {
                this.conditionSetEM_remove_p.DrawProperty("条件集模板 - 移除");
            });
            GameEditorGUILayout.DrawBoxItem(() =>
            {
                var layerSelectorEnable = this.layerSelectorEnable_p.DrawProperty_Bool("层数选择器 开/关");
                if (layerSelectorEnable) this.layerSelectorEM_p.DrawProperty();
            });
            this._DrawActionSORefs();

            this._serializedObject.ApplyModifiedProperties();
        }

        private void _DrawActionSORefs()
        {
            var color = GUI.color;
            GUI.color = Color.green;
            var actionSOs = Resources.LoadAll<GameActionSO>(GameConfigCollection.ACTION_CONFIG_DIR_PATH);
            actionSOs = actionSOs.Filter(actionSO => actionSO.actionType == GameActionType.AttachBuff && actionSO.attachBuffActionEM?.buffSO?.typeId == this.typeId_p.intValue);
            if (actionSOs.Length == 0) return;

            GameEditorGUILayout.DrawBoxItem(() =>
            {
                {
                    EditorGUILayout.LabelField(" -------- 被以下行为使用 --------", EditorStyles.boldLabel);
                    for (int i = 0; i < actionSOs.Length; i++)
                    {
                        var actionSO = actionSOs[i];
                        EditorGUILayout.ObjectField(actionSO.actionEMR.desc.ToString(), actionSO, typeof(GameActionSO), false);
                    }
                }
                GUI.color = color;
            });
        }
    }
}