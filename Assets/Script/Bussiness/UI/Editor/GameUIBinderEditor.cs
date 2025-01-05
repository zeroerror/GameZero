using UnityEngine;
using UnityEditor;
using System.Text;
using System.IO;
using GamePlay.Core;

[CustomEditor(typeof(GameUIBinder))]
public class GameUIBinderEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (GUILayout.Button("生成UI绑定代码"))
        {
            GenerateCode(((GameUIBinder)target).transform);
        }
    }

    private void GenerateCode(Transform root)
    {

        StringBuilder codeBuilder = new StringBuilder();
        codeBuilder.AppendLine("using UnityEngine;");
        codeBuilder.AppendLine();
        codeBuilder.AppendLine("public class GameActionOptionMainViewBinder");
        codeBuilder.AppendLine("{");

        TraverseChildren(root, codeBuilder);

        codeBuilder.AppendLine("}");
        var bindersDir = $"{Application.dataPath}/Script/Bussiness/UI/System/Binders";
        if (!Directory.Exists(bindersDir))
        {
            Directory.CreateDirectory(bindersDir);
        }
        string path = $"{bindersDir}/Game{root.name}Binder.cs";

        // 保存生成的代码到文件
        File.WriteAllText(path, codeBuilder.ToString());

        Debug.Log("代码已生成并保存到: " + path);
        AssetDatabase.Refresh(); // 刷新AssetDatabase以便在Unity中看到新生成的脚本
    }

    private void TraverseChildren(Transform parent, StringBuilder codeBuilder)
    {
        foreach (Transform child in parent)
        {
            string varName = _filter(child.name);
            string varField = $"_{varName}";

            codeBuilder.AppendLine($"    public GameObject {varName} => {varField} ?? ({varField} = GameObject.Find(\"{varName}\"));");
            codeBuilder.AppendLine($"    private GameObject {varField};");

            var isPrefab = PrefabUtility.GetPrefabAssetType(child.gameObject) == PrefabAssetType.Regular;
            if (isPrefab)
            {
                // 为预制体再生成一个绑定类
                GenerateCode(child);
                continue;
            }
            TraverseChildren(child, codeBuilder);
        }
    }

    private string _filter(string str)
    {
        int index = str.IndexOf(' ');
        if (index != -1) str = str.Substring(0, index);
        index = str.IndexOf('(');
        if (index != -1) str = str.Substring(0, index);
        return str;
    }
}