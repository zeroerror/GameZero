using UnityEngine;
using UnityEditor;
using System.Text;
using System.IO;
using System.Collections.Generic;
using UnityEditor.SceneManagement;

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
            var outputDir = $"{Application.dataPath}/Scripts/Bussiness/UI/System/Binders/{prefabName}";
            // 清空之前的绑定类
            if (Directory.Exists(outputDir))
            {
                Directory.Delete(outputDir, true);
            }
            GenerateBinder(root, prefabName + "Binder", outputDir, 0);
            this._binderDict.Clear();
        }

        if (GUILayout.Button("跳转"))
        {
            Selection.activeObject = target;
            EditorGUIUtility.PingObject(target);
        }
    }

    private void GenerateBinder(Transform root, string binderName, string outputDir, int traverseDepth)
    {
        string binderPath = $"{outputDir}/{binderName}.cs";

        StringBuilder codeBuilder = new StringBuilder();
        codeBuilder.AppendLine("using UnityEngine;");
        codeBuilder.AppendLine("using UnityEngine.UI;");
        codeBuilder.AppendLine("using TMPro;");
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

        TraverseChildren(root, codeBuilder, outputDir, binderName, traverseDepth);

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

    private void TraverseChildren(Transform parent, StringBuilder codeBuilder, string outputDirPath, string prefabName, int traverseDepth)
    {
        traverseDepth++;
        foreach (Transform child in parent)
        {
            var depth = traverseDepth;
            string publicVarName = _GetVarName(child, depth);
            string varPath = _GetVarPath(child, depth);
            string privateVarName = $"_{publicVarName}";
            var isBinder = this._IsBinder(child);
            if (isBinder)
            {
                var binderName = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(child.gameObject);
                binderName = Path.GetFileNameWithoutExtension(binderName);
                binderName = $"{binderName}Binder";
                codeBuilder.AppendLine($"    public {binderName} {publicVarName} => {privateVarName} ?? ({privateVarName} = new {binderName}(this.gameObject.transform.Find(\"{varPath}\").gameObject));");
                codeBuilder.AppendLine($"    private {binderName} {privateVarName};");

                var hasBinder = this._binderDict.TryGetValue(binderName, out bool _);
                if (!hasBinder)
                {
                    // 为预制体再生成一个绑定类
                    GenerateBinder(child, binderName, outputDirPath, 0);
                }
                continue;
            }

            var typeName = this._GetUIComTypeName(child);
            if (typeName == "GameObject")
            {
                codeBuilder.AppendLine($"    public {typeName} {publicVarName} => {privateVarName} ?? ({privateVarName} = this.gameObject.transform.Find(\"{varPath}\").gameObject);");
            }
            else
            {
                codeBuilder.AppendLine($"    public {typeName} {publicVarName} => {privateVarName} ?? ({privateVarName} = this.gameObject.transform.Find(\"{varPath}\").GetComponent<{typeName}>());");
            }
            codeBuilder.AppendLine($"    private {typeName} {privateVarName};");

            TraverseChildren(child, codeBuilder, outputDirPath, prefabName, traverseDepth);
        }
    }

    private string _GetUIComTypeName(Transform tf)
    {
        var uiCom = tf.GetComponent<UnityEngine.UI.Graphic>();
        var typeName = uiCom != null ? uiCom.GetType().Name : "GameObject";
        return typeName;
    }

    private bool _IsBinder(Transform tf)
    {
        // 判定是否是预制体
        if (PrefabUtility.IsAnyPrefabInstanceRoot(tf.gameObject))
        {
            return true;
        }

        // 兼容 Prefab 编辑模式下, 无法正确判定是否是预制体的问题
        var prefabStage = PrefabStageUtility.GetCurrentPrefabStage();
        if (prefabStage != null && prefabStage.prefabContentsRoot == tf.gameObject)
        {
            return true;
        }
        return false;
    }

    private string _filter(string str)
    {
        int index = str.IndexOf(' ');
        if (index != -1) str = str.Substring(0, index);
        index = str.IndexOf('(');
        if (index != -1) str = str.Substring(0, index);
        return str;
    }

    /// <summary> 获取变量名 </summary>
    private string _GetVarName(Transform tf, int depth)
    {
        var name = _filter(tf.name);
        if (depth <= 1) return name;
        while (depth > 1)
        {
            depth--;
            tf = tf.parent;
            name = $"{_filter(tf.name)}_{name}";
        }
        return name;
    }

    /// <summary> 获取变量路径 </summary>
    private string _GetVarPath(Transform tf, int depth)
    {
        var path = tf.name;
        if (depth <= 1) return path;
        while (depth > 1)
        {
            depth--;
            tf = tf.parent;
            path = $"{tf.name}/{path}";
        }
        return path;
    }

}