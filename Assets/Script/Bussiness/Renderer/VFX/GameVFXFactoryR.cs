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
            var body = GameObject.Instantiate(prefab);
            var entity = new GameVFXEntityR(body, prefabUrl);
            return entity;
        }
    }
}