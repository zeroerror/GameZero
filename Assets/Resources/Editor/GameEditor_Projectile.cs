using GamePlay.Bussiness.Logic;
using GamePlay.Core;
using UnityEditor;
using UnityEngine;

namespace GamePlay.Config
{
    [CustomEditor(typeof(GameProjectileSO))]
    public class GameEditor_Projectile : Editor
    {
        private SerializedObject _serializedObject;
        private GameProjectileSO so => target as GameProjectileSO;
        private SerializedProperty typeId_p;
        private SerializedProperty projectileName_p;
        private SerializedProperty desc_p;
        private SerializedProperty animClip_p;
        private SerializedProperty prefab_p;
        private SerializedProperty prefabScale_p;
        private SerializedProperty prefabOffset_p;
        private SerializedProperty isLockRotation_p;
        private SerializedProperty animLength_p;
        private SerializedProperty lifeTime_p;
        private SerializedProperty timelineEvents_p;
        private SerializedProperty stateEMs_p;

        private void OnEnable()
        {
            this._serializedObject = new SerializedObject(target);
            this.typeId_p = this._serializedObject.FindProperty("typeId");
            this.projectileName_p = this._serializedObject.FindProperty("projectileName");
            this.desc_p = this._serializedObject.FindProperty("desc");
            this.animClip_p = this._serializedObject.FindProperty("animClip");
            this.prefab_p = this._serializedObject.FindProperty("prefab");
            this.prefabScale_p = this._serializedObject.FindProperty("prefabScale");
            this.prefabOffset_p = this._serializedObject.FindProperty("prefabOffset");
            this.isLockRotation_p = this._serializedObject.FindProperty("isLockRotation");
            this.animLength_p = this._serializedObject.FindProperty("animLength");
            this.lifeTime_p = this._serializedObject.FindProperty("lifeTime");
            this.timelineEvents_p = this._serializedObject.FindProperty("timelineEvents");
            this.stateEMs_p = this._serializedObject.FindProperty("stateEMs");
        }

        public override void OnInspectorGUI()
        {
            this._serializedObject.Update();
            GameEditorGUILayout.DrawBoxItem(() => this._DrawBasicData());
            GameEditorGUILayout.DrawBoxItem(() => this._DrawLogicData());
            GameEditorGUILayout.DrawBoxItem(() => this._DrawStateData());
            GameEditorGUILayout.DrawBoxItem(() => this._DrawActionSORefs());
            this._serializedObject.ApplyModifiedProperties();
        }

        private void _DrawBasicData()
        {
            EditorGUILayout.LabelField("基础信息-------------------------", EditorStyles.boldLabel);
            this.typeId_p.DrawProperty_Int("类型Id");
            this.projectileName_p.DrawProperty_Str("名称");
            this.desc_p.DrawProperty_Str("描述");
            this.animClip_p.DrawProperty<AnimationClip>("动画文件");
            this.prefab_p.DrawProperty<GameObject>("预制体");
            this.prefabScale_p.DrawProperty_Vector2("预制体缩放");
            this.prefabOffset_p.DrawProperty_Vector2("预制体偏移");
            this.isLockRotation_p.DrawProperty_Bool("锁定旋转");
        }

        private void _DrawLogicData()
        {
            EditorGUILayout.LabelField("逻辑数据-------------------------", EditorStyles.boldLabel);
            this.animLength_p.DrawProperty_Float("动画时长(s)");
            this.lifeTime_p.DrawProperty_Float("生命周期(s)");
            var evCount = this.timelineEvents_p?.arraySize ?? 0;
            if (evCount > 0) EditorGUILayout.LabelField($"时间轴事件[{evCount}]");
            EditorGUI.indentLevel += 2;
            for (var i = 0; i < evCount; i++)
            {
                GameEditorGUILayout.DrawBoxItem(() =>
                {
                    var ev_p = this.timelineEvents_p.GetArrayElementAtIndex(i);
                    ev_p.DrawProperty($"事件 {i}");
                });
            }
            EditorGUI.indentLevel -= 2;
        }

        private void _DrawStateData()
        {
            EditorGUILayout.LabelField("状态机-------------------------", EditorStyles.boldLabel);
            // 状态机数组属性
            var stateCount = stateEMs_p?.arraySize ?? 0;
            EditorGUILayout.LabelField($"状态数量: {stateCount}");
            // 绘制所有状态并提供删除按钮
            for (var i = 0; i < stateCount; i++)
            {
                GameEditorGUILayout.DrawBoxItem(() =>
                {
                    var state_p = stateEMs_p.GetArrayElementAtIndex(i);
                    state_p.DrawProperty($"状态 {i}");

                    EditorGUILayout.BeginHorizontal();
                    GameGUILayout.DrawButton("删除", () =>
                    {
                        stateEMs_p.DeleteArrayElementAtIndex(i);
                        i--;
                        stateCount--;
                    }, Color.red, 50);
                    GameGUILayout.DrawButton("重置", () =>
                    {
                        so.stateEMs[i] = new GameProjectileStateEM();
                        this._serializedObject.Update();
                    }, Color.yellow, 50);
                    GameGUILayout.DrawButton("↑", () =>
                    {
                        if (i > 0)
                        {
                            stateEMs_p.MoveArrayElement(i, i - 1);
                        }
                    }, Color.white, 20);
                    GameGUILayout.DrawButton("↓", () =>
                    {
                        if (i < stateCount - 1)
                        {
                            stateEMs_p.MoveArrayElement(i, i + 1);
                        }
                    }, Color.white, 20);
                    EditorGUILayout.EndHorizontal();

                });
            }
            // 添加新状态按钮
            GameGUILayout.DrawButton("添加状态", () =>
            {
                stateEMs_p.InsertArrayElementAtIndex(stateCount);
            }, Color.green, 100);
        }

        private void _DrawActionSORefs()
        {
            var color = GUI.color;
            GUI.color = Color.green;
            var actionSOs = Resources.LoadAll<GameActionSO>(GameConfigCollection.ACTION_CONFIG_DIR_PATH);
            actionSOs = actionSOs.Filter(actionSO => actionSO.actionType == GameActionType.LaunchProjectile && actionSO.launchProjectileActionEM?.launchProjectileSO?.typeId == this.typeId_p.intValue);
            if (actionSOs.Length > 0)
            {
                EditorGUILayout.LabelField(" -------- 被以下行为使用 --------", EditorStyles.boldLabel);
                for (int i = 0; i < actionSOs.Length; i++)
                {
                    var actionSO = actionSOs[i];
                    EditorGUILayout.ObjectField(actionSO.typeId.ToString(), actionSO, typeof(GameActionSO), false);
                }
            }
            GUI.color = color;
        }
    }
}