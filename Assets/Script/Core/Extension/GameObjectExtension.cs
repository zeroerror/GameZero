using UnityEngine;
using UnityEngine.Rendering;

namespace GamePlay.Core
{
    public static class GameObjectExtension
    {
        public static void ForeachChild(this GameObject go, System.Action<GameObject> action)
        {
            if (go == null) return;
            foreach (Transform child in go.transform)
            {
                action(child.gameObject);
            }
        }

        public static void ForeachChild(this Transform tf, System.Action<Transform> action)
        {
            if (tf == null) return;
            foreach (Transform child in tf)
            {
                action(child);
            }
        }

        /// <summary>
        /// 广度遍历GameObject树
        /// </summary>
        public static void Foreach_BFS(this GameObject go, System.Action<GameObject> action)
        {
            if (go == null) return;
            action(go);
            foreach (Transform child in go.transform)
            {
                Foreach_BFS(child.gameObject, action);
            }
        }

        /// <summary>
        /// 深度遍历GameObject树
        /// </summary>
        public static void Foreach_DFS(this GameObject go, System.Action<GameObject> action)
        {
            if (go == null) return;
            foreach (Transform child in go.transform)
            {
                Foreach_DFS(child.gameObject, action);
            }
            action(go);
        }

        /// <summary>
        /// 广度遍历Transform树
        /// </summary>
        public static void Foreach_BFS(this Transform tf, System.Action<Transform> action)
        {
            if (tf == null) return;
            action(tf);
            foreach (Transform child in tf.transform)
            {
                Foreach_BFS(child, action);
            }
        }

        /// <summary>
        /// 深度遍历Transform树
        /// </summary>
        public static void Foreach_DFS(this Transform tf, System.Action<Transform> action)
        {
            if (tf == null) return;
            foreach (Transform child in tf.transform)
            {
                Foreach_DFS(child, action);
            }
            action(tf);
        }

        public static void SetSortingOrder(this GameObject go, int orderInLayer, string layerName = "Default")
        {
            var pss = go.GetComponentsInChildren<ParticleSystem>();
            pss.Foreach((ps) =>
            {
                var render = ps.GetComponent<Renderer>();
                if (render)
                {
                    render.sortingLayerName = layerName;
                    render.sortingOrder = orderInLayer;
                }
            });

            var renderer = go.GetComponent<Renderer>();
            if (renderer)
            {
                renderer.sortingLayerName = layerName;
                renderer.sortingOrder = orderInLayer;
            }

            var sortingGroup = go.GetComponent<SortingGroup>();
            if (sortingGroup)
            {
                sortingGroup.sortingLayerName = layerName;
                sortingGroup.sortingOrder = orderInLayer;
            }
        }

        public static void SetSortingOrder(this Transform tf, float orderInLayer, string layerName = "Default")
        {
            SetSortingOrder(tf, (int)orderInLayer, layerName);
        }
        public static void SetSortingOrder(this Transform tf, int orderInLayer, string layerName = "Default")
        {
            SetSortingOrder(tf.gameObject, orderInLayer, layerName);
        }

        public static bool TryGetSortingOrder(this GameObject go, out int orderInLayer, out string layerName)
        {
            orderInLayer = 0;
            layerName = "Default";
            var renderer = go.GetComponent<Renderer>();
            if (renderer)
            {
                orderInLayer = renderer.sortingOrder;
                layerName = renderer.sortingLayerName;
                return true;
            }

            var sortingGroup = go.GetComponent<SortingGroup>();
            if (sortingGroup)
            {
                orderInLayer = sortingGroup.sortingOrder;
                layerName = sortingGroup.sortingLayerName;
                return true;
            }

            return false;
        }

        public static bool TryGetSortingLayer(this Transform tf, out int orderInLayer, out string layerName)
        {
            return TryGetSortingOrder(tf.gameObject, out orderInLayer, out layerName);
        }

        public static void SetPosZ(this GameObject go, float z)
        {
            var pos = go.transform.position;
            pos.z = z;
            go.transform.position = pos;
        }

        public static void SetPosZ(this Transform tf, float z)
        {
            var pos = tf.position;
            pos.z = z;
            tf.position = pos;
        }

        public static string GetPrefabUrl(this GameObject prefab)
        {
            var url = prefab ? UnityEditor.AssetDatabase.GetAssetPath(prefab) : null;
            if (url != null)
            {
                url = System.Text.RegularExpressions.Regex.Replace(url, @"Assets/Resources/", "");
                url = url.Substring(0, url.LastIndexOf('.'));
            }
            return url;
        }

        public static string GetResRelativeUrl(this UnityEngine.Object obj, bool withExtension = false)
        {
            var url = UnityEditor.AssetDatabase.GetAssetPath(obj);
            url = System.Text.RegularExpressions.Regex.Replace(url, @"Assets/Resources/", "");
            if (!withExtension)
            {
                url = url.Substring(0, url.LastIndexOf('.'));
            }
            return url;
        }
    }
}