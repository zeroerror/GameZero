using UnityEditor;
using UnityEngine;

namespace GamePlay.Config
{
    public static class GamePropertyDrawerExtension
    {
        private static void _AdjustLayout(float height)
        {
            EditorGUILayout.Space(height);
        }

        public static T DrawProperty<T>(this SerializedProperty property, string label = "", float height = 6, bool isReadOnly = false) where T : Object
        {
            DrawProperty(property, label, height, isReadOnly);
            return property.objectReferenceValue as T;
        }


        public static void DrawProperty(this SerializedProperty property, string label = "", float height = 6, bool isReadOnly = false)
        {
            EditorGUI.BeginChangeCheck();
            if (isReadOnly)
                EditorGUILayout.PropertyField(property, new GUIContent(label), true);
            else
                EditorGUILayout.PropertyField(property, new GUIContent(label));

            if (EditorGUI.EndChangeCheck())
            {
                property.serializedObject.ApplyModifiedProperties();
            }
            _AdjustLayout(height);
        }


        public static int DrawProperty_Int(this SerializedProperty property, string label, float height = 6, bool isReadOnly = false)
        {
            if (isReadOnly) EditorGUILayout.IntField(label, property.intValue);
            else property.intValue = EditorGUILayout.IntField(label, property.intValue);
            _AdjustLayout(height);
            return property.intValue;
        }

        public static float DrawProperty_Float(this SerializedProperty property, string label, float height = 6, bool isReadOnly = false)
        {
            if (isReadOnly) EditorGUILayout.FloatField(label, property.floatValue);
            else property.floatValue = EditorGUILayout.FloatField(label, property.floatValue);
            _AdjustLayout(height);
            return property.floatValue;
        }

        public static Vector2 DrawProperty_Vector2(this SerializedProperty property, string label, float height = 6, bool isReadOnly = false)
        {
            if (isReadOnly) EditorGUILayout.Vector2Field(label, property.vector2Value);
            else property.vector2Value = EditorGUILayout.Vector2Field(label, property.vector2Value);
            _AdjustLayout(height);
            return property.vector2Value;
        }

        public static Vector3 DrawProperty_Vector3(this SerializedProperty property, string label, float height = 6, bool isReadOnly = false)
        {
            if (isReadOnly) EditorGUILayout.Vector3Field(label, property.vector3Value);
            else property.vector3Value = EditorGUILayout.Vector3Field(label, property.vector3Value);
            _AdjustLayout(height);
            return property.vector3Value;
        }

        public static T DrawProperty_EnumPopup<T>(this SerializedProperty property, string label, float height = 6, bool isReadOnly = false) where T : System.Enum
        {
            var position = EditorGUILayout.GetControlRect();

            // 获取当前的枚举值
            T currentEnumValue = (T)System.Enum.GetValues(typeof(T)).GetValue(property.enumValueIndex);

            // 显示枚举弹出框并更新枚举值
            currentEnumValue = (T)EditorGUI.EnumPopup(position, label, currentEnumValue);

            // 如果只读，直接显示枚举值，但不进行修改
            if (isReadOnly)
            {
                EditorGUILayout.LabelField(label, currentEnumValue.ToString());
            }
            else
            {
                // 更新序列化属性的枚举索引
                property.enumValueIndex = System.Array.IndexOf(System.Enum.GetValues(typeof(T)), currentEnumValue);
            }

            // 调整布局
            _AdjustLayout(height);

            return currentEnumValue;
        }

        public static bool DrawProperty_Bool(this SerializedProperty property, string label, float height = 6, bool isReadOnly = false)
        {
            if (isReadOnly) EditorGUILayout.Toggle(label, property.boolValue);
            else property.boolValue = EditorGUILayout.Toggle(label, property.boolValue);
            _AdjustLayout(height);
            return property.boolValue;
        }

        public static string DrawProperty_Str(this SerializedProperty property, string label, float height = 6, bool isReadOnly = false)
        {
            if (isReadOnly) EditorGUILayout.TextField(label, property.stringValue);
            else property.stringValue = EditorGUILayout.TextField(label, property.stringValue);
            _AdjustLayout(height);
            return property.stringValue;
        }
    }
}
