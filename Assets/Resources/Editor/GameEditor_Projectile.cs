using Codice.Client.BaseCommands;
using UnityEditor;
using UnityEngine;

namespace GamePlay.Config
{
    [CustomEditor(typeof(GameProjectileSO))]
    public class GameEditor_Projectile : Editor
    {
        public override void OnInspectorGUI()
        {
            EditorGUI.BeginChangeCheck();

            GameProjectileSO so = (GameProjectileSO)target;
            GameEditorGUILayout.DrawBoxItem(() => this._DrawBasicData(so));
            GameEditorGUILayout.DrawBoxItem(() => this._DrawLogicData(so));
            GameEditorGUILayout.DrawBoxItem(() => this._DrawStateData(so));

            if (EditorGUI.EndChangeCheck())
            {
                EditorUtility.SetDirty(so);
            }
        }

        private void _DrawBasicData(GameProjectileSO so)
        {
            EditorGUILayout.LabelField("基础信息-------------------------", EditorStyles.boldLabel);
            so.typeId = EditorGUILayout.IntField("类型Id", so.typeId);
            so.projectileName = EditorGUILayout.TextField("名称", so.projectileName);
            so.desc = EditorGUILayout.TextField("描述", so.desc);

            EditorGUI.BeginChangeCheck();
            so.animClip = (AnimationClip)EditorGUILayout.ObjectField("动画文件", so.animClip, typeof(AnimationClip), false);
            if (EditorGUI.EndChangeCheck()) so.UpdateData();

            so.prefab = (GameObject)EditorGUILayout.ObjectField("预制体", so.prefab, typeof(GameObject), false);
            so.prefabScale = EditorGUILayout.Vector2Field("预制体缩放", so.prefabScale);
            so.prefabOffset = EditorGUILayout.Vector2Field("预制体偏移", so.prefabOffset);
        }

        private void _DrawLogicData(GameProjectileSO so)
        {
            EditorGUILayout.LabelField("逻辑数据-------------------------", EditorStyles.boldLabel);
            so.animLength = EditorGUILayout.FloatField("动画时长(s)", so.animLength);
            so.lifeTime = EditorGUILayout.FloatField("生命周期(s)", so.lifeTime);
            var timelineEvents_p = serializedObject.FindProperty("timelineEvents");
            var evCount = timelineEvents_p?.arraySize ?? 0;
            if (evCount > 0) EditorGUILayout.LabelField($"时间轴事件[{evCount}]");
            EditorGUI.indentLevel += 2;
            for (var i = 0; i < evCount; i++)
            {
                GameEditorGUILayout.DrawBoxItem(() =>
                {
                    var ev_p = timelineEvents_p.GetArrayElementAtIndex(i);
                    ev_p.DrawProperty($"事件 {i}");
                });
            }
            EditorGUI.indentLevel -= 2;
        }

        private void _DrawStateData(GameProjectileSO so)
        {
            EditorGUILayout.LabelField("状态机-------------------------", EditorStyles.boldLabel);
            // 状态机数组属性
            var stateEMs_p = serializedObject.FindProperty("stateEMs");
            // 显示当前状态的数量
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
                        serializedObject.Update();
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
    }
}