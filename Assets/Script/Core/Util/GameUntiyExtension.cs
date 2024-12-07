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
    }

}