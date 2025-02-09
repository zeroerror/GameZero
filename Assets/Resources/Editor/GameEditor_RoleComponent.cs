using GamePlay.Config;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GameRoleComponent))]
public class GameEditor_RoleComponent : Editor
{

    private SerializedObject _serializedObject;
    private SerializedProperty roleSO_p;

    private void OnEnable()
    {
        _serializedObject = new SerializedObject(target);
        roleSO_p = _serializedObject.FindProperty("roleSO");
    }

    public override void OnInspectorGUI()
    {
        this._serializedObject.Update();

        var roleSO = roleSO_p.DrawProperty<GameRoleSO>("角色模板");
        var skillSOs = roleSO?.skills;
        if (skillSOs != null)
        {
            EditorGUILayout.LabelField("技能列表");
            for (int i = 0; i < skillSOs.Length; i++)
            {
                var skillSO = skillSOs[i];
                EditorGUILayout.ObjectField(skillSO, typeof(GameSkillSO), false);
            }
        }
        // 保存按钮
        if (GUILayout.Button("保存"))
        {
            var roleComponent = target as GameRoleComponent;
            roleComponent.Save();
        }

        this._serializedObject.ApplyModifiedProperties();
    }
}