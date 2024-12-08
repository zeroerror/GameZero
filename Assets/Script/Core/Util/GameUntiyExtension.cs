using UnityEngine;

namespace GamePlay.Core
{
    public static class GameUntiyExtension
    {
        /// <summary>
        /// 广度遍历GameObject树
        /// </summary>
        public static void ForeachGameObject_BFS(this GameObject go, System.Action<GameObject> action)
        {
            if (go == null) return;
            action(go);
            foreach (Transform child in go.transform)
            {
                ForeachGameObject_BFS(child.gameObject, action);
            }
        }

        /// <summary>
        /// 深度遍历GameObject树
        /// </summary>
        public static void ForeachGameObject_DFS(this GameObject go, System.Action<GameObject> action)
        {
            if (go == null) return;
            foreach (Transform child in go.transform)
            {
                ForeachGameObject_DFS(child.gameObject, action);
            }
            action(go);
        }

        /// <summary>
        /// 广度遍历Transform树
        /// </summary>
        public static void ForeachTransfrom_BFS(this Transform tf, System.Action<Transform> action)
        {
            if (tf == null) return;
            action(tf);
            foreach (Transform child in tf.transform)
            {
                ForeachTransfrom_BFS(child, action);
            }
        }

        /// <summary>
        /// 深度遍历Transform树
        /// </summary>
        public static void ForeachTransfrom_DFS(this Transform tf, System.Action<Transform> action)
        {
            if (tf == null) return;
            foreach (Transform child in tf.transform)
            {
                ForeachTransfrom_DFS(child, action);
            }
            action(tf);
        }

        public static void SetSortingLayer(this GameObject go, int orderInLayer, string layerName = "Default")
        {
            var renderer = go.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.sortingLayerName = layerName;
                renderer.sortingOrder = orderInLayer;
            }
        }

        public static void SetSortingLayer(this Transform tf, float orderInLayer, string layerName = "Default")
        {
            SetSortingLayer(tf, (int)orderInLayer, layerName);
        }
        public static void SetSortingLayer(this Transform tf, int orderInLayer, string layerName = "Default")
        {
            var renderer = tf.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.sortingLayerName = layerName;
                renderer.sortingOrder = orderInLayer;
            }
        }

        public static bool GetSortingLayer(this GameObject go, out int orderInLayer, out string layerName)
        {
            orderInLayer = 0;
            layerName = "Default";
            var renderer = go.GetComponent<Renderer>();
            if (renderer == null) return false;
            orderInLayer = renderer.sortingOrder;
            layerName = renderer.sortingLayerName;
            return true;
        }

        public static bool TryGetSortingLayer(this Transform tf, out int orderInLayer, out string layerName)
        {
            orderInLayer = 0;
            layerName = "Default";
            var renderer = tf.GetComponent<Renderer>();
            if (renderer == null) return false;
            orderInLayer = renderer.sortingOrder;
            layerName = renderer.sortingLayerName;
            return true;
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
    }
}