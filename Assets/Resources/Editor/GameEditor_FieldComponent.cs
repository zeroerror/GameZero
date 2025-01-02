using GamePlay.Config;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GameFieldComponent))]
public class GameEditor_FieldComponent : Editor
{
    public override void OnInspectorGUI()
    {
        // 绘制默认的Inspector
        DrawDefaultInspector();

        // 获取目标对象
        GameFieldComponent gameFieldComponent = (GameFieldComponent)target;

        // 添加一个按钮
        if (GUILayout.Button("保存"))
        {
            // 调用保存方法
            gameFieldComponent.Save();
        }
    }
}