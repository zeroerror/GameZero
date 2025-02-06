using UnityEngine;
using UnityEditor;
using System.Text;
using System.IO;
using System.Collections.Generic;
using System;

[CustomEditor(typeof(UIBinder))]
public class UIBinderEditor : Editor
{
    private Dictionary<string, bool> _binderDict = new Dictionary<string, bool>();

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (GUILayout.Button("生成UI绑定代码"))
        {
            var root = ((UIBinder)target).transform;
            string prefabName = _filter(root.name);
            var outputDir = $"{Application.dataPath}/Script/Bussiness/UI/System/Binders/{prefabName}";
            // 清空之前的绑定类
            if (Directory.Exists(outputDir))
            {
                Directory.Delete(outputDir, true);
            }
            GenerateBinder(root, prefabName + "Binder", outputDir);
            this._binderDict.Clear();
        }

        if (GUILayout.Button("跳转"))
        {
            Selection.activeObject = target;
            EditorGUIUtility.PingObject(target);
        }
    }

    private void GenerateBinder(Transform root, string binderName, string outputDir)
    {
        string binderPath = $"{outputDir}/{binderName}.cs";

        StringBuilder codeBuilder = new StringBuilder();
        codeBuilder.AppendLine("using UnityEngine;");
        codeBuilder.AppendLine();
        codeBuilder.AppendLine($"public class {binderName}");
        codeBuilder.AppendLine("{");
        codeBuilder.AppendLine("    public GameObject gameObject{ get; private set; }");
        codeBuilder.AppendLine();
        codeBuilder.AppendLine($"    public {binderName}(GameObject gameObject)");
        codeBuilder.AppendLine("    {");
        codeBuilder.AppendLine("        this.gameObject = gameObject;");
        codeBuilder.AppendLine("    }");
        codeBuilder.AppendLine();

        TraverseChildren(root, codeBuilder, outputDir, binderName);

        codeBuilder.AppendLine("}");

        if (!Directory.Exists(outputDir))
        {
            Directory.CreateDirectory(outputDir);
        }

        // 保存生成的代码到文件
        File.WriteAllText(binderPath, codeBuilder.ToString());
        AssetDatabase.Refresh(); // 刷新AssetDatabase以便在Unity中看到新生成的脚本
        this._binderDict.TryAdd(binderName, true);
        Debug.Log("代码已生成并保存到: " + binderPath);
    }

    private void TraverseChildren(Transform parent, StringBuilder codeBuilder, string outputDirPath, string prefabName)
    {
        foreach (Transform child in parent)
        {
            string varName = _filter(child.name);
            string varField = $"_{varName}";
            var isBinder = PrefabUtility.IsAnyPrefabInstanceRoot(child.gameObject);
            if (isBinder)
            {
                var binderName = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(child.gameObject);
                binderName = Path.GetFileNameWithoutExtension(binderName);
                binderName = $"{binderName}Binder";
                codeBuilder.AppendLine($"    public {binderName} {varName} => {varField} ?? ({varField} = new {binderName}(GameObject.Find(\"{varName}\")));");
                codeBuilder.AppendLine($"    private {binderName} {varField};");

                var hasBinder = this._binderDict.TryGetValue(binderName, out bool _);
                if (!hasBinder)
                {
                    // 为预制体再生成一个绑定类
                    GenerateBinder(child, binderName, outputDirPath);
                }
                continue;
            }

            var typeName = "GameObject";
            codeBuilder.AppendLine($"    public {typeName} {varName} => {varField} ?? ({varField} = this.gameObject.transform.Find(\"{varName}\").gameObject);");
            codeBuilder.AppendLine($"    private {typeName} {varField};");
            TraverseChildren(child, codeBuilder, outputDirPath, prefabName);
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