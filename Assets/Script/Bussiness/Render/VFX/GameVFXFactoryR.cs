using GamePlay.Core;
using UnityEngine;

namespace GamePlay.Bussiness.Render
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
            var body = GameObject.Instantiate(prefab);
            var root = new GameObject();
            body.transform.SetParent(root.transform);
            body.name = "Body";
            var entity = new GameVFXEntityR(root, body, prefabUrl);
            return entity;
        }
    }
}