using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEditor.ShaderUtil;

namespace GamePlay.Config
{
    [CustomPropertyDrawer(typeof(GameShaderEffectPropTimeLineEM))]
    public class GamePropertyDrawer_GameShaderEffectArgsTimeLine : GamePropertyDrawer
    {
        protected override void _OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // 获取shader的参数列表
            var propDict = _GetShaderPropDict(property);
            var propName_p = property.FindPropertyRelative("propName");

            // 如果字典不为空，展示参数下拉框
            if (propDict.Count > 0)
            {
                // 将字典中的属性名称提取为字符串数组
                List<string> propNames = new List<string>(propDict.Keys);
                string[] options = propNames.ToArray();

                // 获取当前选中的索引
                string currentSelection = propName_p.stringValue;
                int selectedIndex = propNames.IndexOf(currentSelection);

                // 确保选中的索引有效
                if (selectedIndex == -1) selectedIndex = 0;

                // 显示下拉框并获取选中的索引
                options.DrawPopup(ref selectedIndex, "参数名称");

                // 更新选中的值
                propName_p.stringValue = options[selectedIndex];
            }
            else
            {
                EditorGUI.LabelField(position, label.text, "不存在可选参数");
                return;
            }

            // 时间区间
            property.FindPropertyRelative("startTime").DrawProperty_Float("开始时间");
            property.FindPropertyRelative("endTime").DrawProperty_Float("结束时间");

            // 根据选中的参数类型，展示不同的输入框
            var propName = propName_p.stringValue;
            var propType = propDict[propName];
            switch (propType)
            {
                case ShaderPropertyType.Float:
                case ShaderPropertyType.Range:
                    property.FindPropertyRelative("fromValue_float").DrawProperty_Float("起始值");
                    property.FindPropertyRelative("toValue_float").DrawProperty_Float("结束值");
                    property.FindPropertyRelative("curve_float").DrawProperty_AnimationCurve("曲线");
                    break;
                case ShaderPropertyType.Color:
                    property.FindPropertyRelative("fromValue_color").DrawProperty_Color("起始值");
                    property.FindPropertyRelative("toValue_color").DrawProperty_Color("结束值");
                    break;
                default:
                    EditorGUILayout.HelpBox($"不支持的参数类型: {propType}", MessageType.Warning);
                    break;
            }
        }

        private Dictionary<string, object> _GetShaderPropDict(SerializedProperty property)
        {
            var propDict = new Dictionary<string, object>();
            var material_p = property.FindPropertyRelative("material");
            if (material_p.objectReferenceValue)
            {
                var material = material_p.objectReferenceValue as Material;
                var shader = material.shader;
                var count = GetPropertyCount(shader);
                for (int i = 0; i < count; i++)
                {
                    var propName = GetPropertyName(shader, i);
                    var propType = GetPropertyType(shader, i);
                    propDict[propName] = propType;
                }
            }
            return propDict;
        }
    }
}