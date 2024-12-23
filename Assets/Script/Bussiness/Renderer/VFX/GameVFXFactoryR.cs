using GamePlay.Core;
using UnityEngine;

namespace GamePlay.Bussiness.Renderer
{
    public class GameVFXFactoryR
    {
        public GameVFXFactoryR()
        {
        }

        public GameVFXEntityR Load(string prefabUrl)
        {
            var prefab = Resources.Load<GameObject>(prefabUrl);
            if (prefab == null)
            {
                GameLogger.LogError($"VFX加载失败：{prefabUrl}");
                return null;
            }
            var root = new GameObject();
            var go = GameObject.Instantiate(prefab);
            go.transform.SetParent(root.transform);
            var entity = new GameVFXEntityR(root, prefabUrl);
            return entity;
        }
    }
}