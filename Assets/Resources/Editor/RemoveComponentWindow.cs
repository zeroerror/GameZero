using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

namespace GamePlay.Config
{
    public class RemoveComponentWindow : EditorWindow
    {
        private List<GameObject> selectedPrefabs = new List<GameObject>();

        [MenuItem("Tools/批量移除预制体组件")]
        public static void ShowWindow()
        {
            GetWindow<RemoveComponentWindow>("移除组件工具");
        }

        private void OnGUI()
        {
            GUILayout.Label("批量移除预制体组件", EditorStyles.boldLabel);

            if (GUILayout.Button("获取选中的预制体"))
            {
                selectedPrefabs.Clear();
                foreach (var obj in Selection.gameObjects)
                {
                    if (PrefabUtility.IsPartOfPrefabAsset(obj))
                    {
                        selectedPrefabs.Add(obj);
                    }
                }
                Debug.Log($"找到 {selectedPrefabs.Count} 个预制体");
            }

            EditorGUILayout.Space();

            if (GUILayout.Button("移除指定组件"))
            {
                RemoveComponentFromPrefabs();
            }

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("选中的预制体列表:", EditorStyles.boldLabel);

            foreach (var prefab in selectedPrefabs)
            {
                EditorGUILayout.LabelField(prefab.name);
            }
        }

        private void RemoveComponentFromPrefabs()
        {
            foreach (var prefab in selectedPrefabs)
            {
                GameObject instance = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
                if (instance == null) continue;

                // 遍历所有子物体删除组件
                RemoveComponentRecursively(instance.transform);

                // 应用修改到预制体
                PrefabUtility.SaveAsPrefabAsset(instance, AssetDatabase.GetAssetPath(prefab));
                DestroyImmediate(instance);
            }

            AssetDatabase.Refresh();
        }

        private void RemoveComponentRecursively(Transform obj)
        {
            GameObjectUtility.RemoveMonoBehavioursWithMissingScript(obj.gameObject);
            var coms = obj.GetComponents<Component>();
            foreach (var com in coms)
            {
                if (com == null) continue;
                if (Array.Exists(keepTypes, t => t == com.GetType())) continue;
                DestroyImmediate(com);
            }

            foreach (Transform child in obj)
            {
                RemoveComponentRecursively(child);
            }
        }
        /// <summary> 保留的组件类型 </summary>
        private Type[] keepTypes = new Type[]
        {
            typeof(Transform),
            typeof(SpriteRenderer),
        };
    }
}
