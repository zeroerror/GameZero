using System.Collections.Generic;
using GamePlay.Bussiness.Logic;
using GamePlay.Core;
using UnityEditor;
using UnityEngine;

namespace GamePlay.Config
{
    [CustomPropertyDrawer(typeof(GameSkillMovementEM))]
    public class GamePropertyDrawer_SkillMovement : GamePropertyDrawer
    {
        protected override void _OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUILayout.LabelField("以下参数由动画提取------------------");
            var movementType = property.FindPropertyRelative("movementType").DrawProperty_EnumPopup<GameSkillMovementType>("移动类型");
            var speedEMs_p = property.FindPropertyRelative("speedEMs");
            var speedEMList = new List<GameSKillDashEM>();
            var maxPositiveDis = 0f;
            var maxPositiveDisIdx = -1;
            if (movementType != GameSkillMovementType.None)
            {
                var clip = property.FindPropertyRelative("clip").objectReferenceValue as AnimationClip;
                var bindings = AnimationUtility.GetCurveBindings(clip);
                bindings?.Foreach((binding, index) =>
                {
                    var curve = AnimationUtility.GetEditorCurve(clip, binding);
                    if (curve == null) return;
                    var isTarget = binding.path == "Right" || binding.path == "Body";
                    if (!isTarget) return;
                    var isTransform = binding.type == typeof(Transform);
                    if (!isTransform) return;
                    var isX = binding.propertyName == "m_LocalPosition.x";
                    var isY = binding.propertyName == "m_LocalPosition.y";
                    if (!isX && !isY) return;
                    var keys = curve.keys;
                    for (int i = 0; i < keys.Length; i++)
                    {
                        // 冲刺距离, 取x轴正向最大值
                        var key = keys[i];
                        if (key.value > maxPositiveDis)
                        {
                            maxPositiveDis = key.value;
                            maxPositiveDisIdx = i;
                        }
                        var speedEM = i < speedEMList.Count ? speedEMList[i] : null;
                        if (speedEM == null)
                        {
                            speedEM = new GameSKillDashEM();
                            speedEMList.Add(speedEM);
                        }
                        speedEM.time = keys[i].time.ToFixed(2);
                        speedEM.frame = keys[i].time.ToFrame();
                        if (isX) speedEM.x = keys[i].value;
                        else if (isY) speedEM.y = keys[i].value;
                    }
                });
            }
            var emCount = speedEMList?.Count ?? 0;
            if (emCount > 0)
            {
                // 时间轴信息
                var color = GUI.color;
                speedEMs_p.arraySize = speedEMList.Count;
                for (int i = 0; i < speedEMList.Count; i++)
                {
                    var em = speedEMList[i];
                    em.distanceRatio = em.x / maxPositiveDis;

                    var em_p = speedEMs_p.GetArrayElementAtIndex(i);
                    em_p.FindPropertyRelative("time").floatValue = em.time;
                    em_p.FindPropertyRelative("frame").intValue = em.frame;
                    em_p.FindPropertyRelative("x").floatValue = em.x;
                    em_p.FindPropertyRelative("y").floatValue = em.y;
                    em_p.FindPropertyRelative("distanceRatio").floatValue = em.distanceRatio;
                    GUI.color = i == maxPositiveDisIdx ? Color.red : Color.green;
                    EditorGUILayout.LabelField($"[{em.time}s/{em.frame}帧]: ({em.x.ToFixed(2)}, {em.y.ToFixed(2)}) 距离比例{em.distanceRatio.ToFixed(2)}");
                }
                // 冲刺距离
                var dashDistance_p = property.FindPropertyRelative("dashDistance");
                dashDistance_p.floatValue = maxPositiveDis;
                GUI.color = Color.green;
                dashDistance_p.DrawProperty_Float("冲刺距离", 4, true);
                GUI.color = color;
            }
        }
    }
}