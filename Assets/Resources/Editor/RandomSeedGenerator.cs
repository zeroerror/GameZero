using UnityEngine;
using UnityEditor;
using System.IO;

public class RandomSeedGenerator : EditorWindow
{
    private int seedCount = 10000; // 默认生成 10000 个随机种子
    private string savePath = "Assets/Resources/random_seeds.txt";

    [MenuItem("Tools/生成随机种子文件")]
    public static void ShowWindow()
    {
        GetWindow<RandomSeedGenerator>("随机种子生成器");
    }

    private void OnGUI()
    {
        GUILayout.Label("随机种子文件生成", EditorStyles.boldLabel);

        // 输入种子数量
        seedCount = EditorGUILayout.IntField("种子数量", seedCount);

        // 显示保存路径
        EditorGUILayout.LabelField("保存路径:", savePath);

        if (GUILayout.Button("生成随机种子文件"))
        {
            GenerateSeedFile();
        }
    }

    private void GenerateSeedFile()
    {
        // 确保 Resources 目录存在
        string directory = Path.GetDirectoryName(savePath);
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        // 生成随机种子并写入文件
        using (StreamWriter writer = new StreamWriter(savePath))
        {
            for (int i = 0; i < seedCount; i++)
            {
                int seed = Random.Range(0, int.MaxValue);

                // 如果是最后一个种子，不加逗号
                if (i == seedCount - 1)
                    writer.Write(seed);
                else
                    writer.Write(seed + ",");
            }
        }

        // 刷新 Unity 资源
        AssetDatabase.Refresh();
        Debug.Log($"随机种子文件已生成: {savePath}");
    }
}
