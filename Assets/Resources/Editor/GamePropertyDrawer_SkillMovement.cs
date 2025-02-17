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
            var speedEMList = new List<GameSKillDashSpeedEM>();
            var maxPositiveDis = 0f;
            var maxPositiveDisIdx = -1;
            if (movementType != GameSkillMovementType.None)
            {
                var clip = property.FindPropertyRelative("clip").objectReferenceValue as AnimationClip;
                var bindings = AnimationUtility.GetCurveBindings(clip);
                var count = 0;
                bindings?.Foreach((binding, index) =>
                {
                    var curve = AnimationUtility.GetEditorCurve(clip, binding);
                    if (curve == null) return;
                    var isTarget = binding.path == "Right" || binding.path == "Body";
                    if (!isTarget) return;
                    var isTransform = binding.type == typeof(Transform);
                    if (!isTransform) return;
                    var isTransX = binding.propertyName == "m_LocalPosition.x";
                    if (!isTransX) return;
                    var keys = curve.keys;
                    EditorGUILayout.LabelField($"时间轴[{++count}] - {binding.path}");
                    for (int i = 0; i < keys.Length; i++)
                    {
                        // 冲刺距离, 取x轴正向最大值
                        var key = keys[i];
                        if (key.value > maxPositiveDis)
                        {
                            maxPositiveDis = key.value;
                            maxPositiveDisIdx = i;
                        }

                        var isLastElement = i == keys.Length - 1;
                        if (isLastElement)
                        {
                            speedEMList.Add(new GameSKillDashSpeedEM
                            {
                                time = keys[i].time.ToFixed(2),
                                frame = keys[i].time.ToFrame(),
                                x = key.value,
                            });
                            break;
                        }

                        var nextKey = keys[i + 1];
                        var time = key.time;
                        var timeoffset = nextKey.time - key.time;
                        if (timeoffset <= 0) return;
                        var disOffset = nextKey.value - key.value;
                        var speed = disOffset / timeoffset;
                        var frame = time.ToFrame();
                        speedEMList.Add(new GameSKillDashSpeedEM
                        {
                            time = keys[i].time.ToFixed(2),
                            frame = frame,
                            x = key.value,
                        });
                    }
                });
            }
            var emCount = speedEMList?.Count ?? 0;
            if (emCount > 0)
            {
                // 时间轴信息
                speedEMs_p.arraySize = speedEMList.Count;
                for (int i = 0; i < speedEMList.Count; i++)
                {
                    var em = speedEMList[i];
                    em.distanceRatio = em.x / maxPositiveDis;

                    var em_p = speedEMs_p.GetArrayElementAtIndex(i);
                    em_p.FindPropertyRelative("time").floatValue = em.time;
                    em_p.FindPropertyRelative("frame").intValue = em.frame;
                    em_p.FindPropertyRelative("x").floatValue = em.x;
                    em_p.FindPropertyRelative("distanceRatio").floatValue = em.distanceRatio;
                    EditorGUILayout.LabelField($"{em.time}秒({em.frame}帧): 距离比例{em.distanceRatio}");
                }
                // 冲刺距离
                var dashDistance_p = property.FindPropertyRelative("dashDistance");
                dashDistance_p.floatValue = maxPositiveDis;
                dashDistance_p.DrawProperty_Float("冲刺距离", 4, true);
            }
        }
    }
}