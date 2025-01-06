using UnityEngine;
using UnityEditor;
using System.Text;
using System.IO;
using System.Collections.Generic;

[CustomEditor(typeof(GameUIBinder))]
public class GameUIBinderEditor : Editor
{
    private Dictionary<string, bool> _binderDict = new Dictionary<string, bool>();

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (GUILayout.Button("生成UI绑定代码"))
        {
            var root = ((GameUIBinder)target).transform;
            string prefabName = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(root);
            prefabName = Path.GetFileNameWithoutExtension(prefabName);
            var outputDir = $"{Application.dataPath}/Script/Bussiness/UI/System/Binders/{prefabName}";
            // 清空之前的绑定类
            if (Directory.Exists(outputDir))
            {
                Directory.Delete(outputDir, true);
            }
            GenerateBinder(root, outputDir);
            this._binderDict.Clear();
        }

        if (GUILayout.Button("跳转"))
        {
            Selection.activeObject = target;
            EditorGUIUtility.PingObject(target);
        }
    }

    private void GenerateBinder(Transform root, string outputDir)
    {
        string prefabName = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(root);
        prefabName = Path.GetFileNameWithoutExtension(prefabName);

        string binderPath = $"{outputDir}/Game{prefabName}Binder.cs";

        StringBuilder codeBuilder = new StringBuilder();
        codeBuilder.AppendLine("using UnityEngine;");
        codeBuilder.AppendLine();
        codeBuilder.AppendLine($"public class Game{prefabName}Binder");
        codeBuilder.AppendLine("{");
        codeBuilder.AppendLine("    private GameObject _gameObject;");
        codeBuilder.AppendLine();
        codeBuilder.AppendLine($"    public Game{prefabName}Binder(GameObject gameObject)");
        codeBuilder.AppendLine("    {");
        codeBuilder.AppendLine("        _gameObject = gameObject;");
        codeBuilder.AppendLine("    }");
        codeBuilder.AppendLine();

        TraverseChildren(root, codeBuilder, outputDir, prefabName);

        codeBuilder.AppendLine("}");

        if (!Directory.Exists(outputDir))
        {
            Directory.CreateDirectory(outputDir);
        }

        // 保存生成的代码到文件
        File.WriteAllText(binderPath, codeBuilder.ToString());
        AssetDatabase.Refresh(); // 刷新AssetDatabase以便在Unity中看到新生成的脚本
        this._binderDict.TryAdd(prefabName, true);
        Debug.Log("代码已生成并保存到: " + binderPath);
    }

    private void TraverseChildren(Transform parent, StringBuilder codeBuilder, string outputDirPath, string prefabName)
    {
        foreach (Transform child in parent)
        {
            string varName = _filter(child.name);
            string varField = $"_{varName}";


            var isBinder = PrefabUtility.IsAnyPrefabInstanceRoot(child.gameObject);
            if (!isBinder)
            {
                var typeName = "GameObject";
                codeBuilder.AppendLine($"    public {typeName} {varName} => {varField} ?? ({varField} = _gameObject.transform.Find(\"{varName}\").gameObject);");
                codeBuilder.AppendLine($"    private {typeName} {varField};");
            }
            else
            {
                var typeName = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(child);
                typeName = Path.GetFileNameWithoutExtension(typeName);
                typeName = $"Game{typeName}Binder";
                codeBuilder.AppendLine($"    public {typeName} {varName} => {varField} ?? ({varField} = new {typeName}(GameObject.Find(\"{varName}\")));");
                codeBuilder.AppendLine($"    private {typeName} {varField};");
            }

            var hasBinder = this._binderDict.TryGetValue(prefabName, out bool _);
            if (isBinder && !hasBinder)
            {
                // 为预制体再生成一个绑定类
                GenerateBinder(child, outputDirPath);
                continue;
            }
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