using GamePlay.Core;
using UnityEditor;
using UnityEngine;

namespace GamePlay.Config
{
    [CustomEditor(typeof(GameShaderEffectSO))]
    public class GameEditor_ShaderEffect : Editor
    {
        private SerializedObject _serializedObject;
        private SerializedProperty typeId_p;
        private SerializedProperty desc_p;
        private SerializedProperty material_p;
        private SerializedProperty loopCount_p;
        private SerializedProperty propTimeLines_p;

        private void OnEnable()
        {
            this._serializedObject = new SerializedObject(target);
            this.typeId_p = _serializedObject.FindProperty("typeId");
            this.desc_p = _serializedObject.FindProperty("desc");
            this.material_p = _serializedObject.FindProperty("material");
            this.loopCount_p = _serializedObject.FindProperty("loopCount");
            this.propTimeLines_p = _serializedObject.FindProperty("propTimeLines");
        }

        public override void OnInspectorGUI()
        {
            this._serializedObject.Update();

            this.typeId_p.DrawProperty_Int("类型ID");
            this.desc_p.DrawProperty_Str("描述");
            var material = this.material_p.DrawProperty<Material>("材质");
            this.loopCount_p.DrawProperty_Int("循环次数(0表示无限循环)");
            if (material)
            {
                // 设置参数时间轴的material
                foreach (SerializedProperty propTimeLine_p in this.propTimeLines_p)
                {
                    propTimeLine_p.FindPropertyRelative("material").objectReferenceValue = material;
                }
                this.propTimeLines_p.DrawProperty_Array("参数时间轴");
            }
            else
            {
                EditorGUILayout.HelpBox("材质为空", MessageType.Warning);
            }
            this._serializedObject.ApplyModifiedProperties();
        }

    }
}