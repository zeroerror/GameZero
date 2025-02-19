using GamePlay.Core;
using GamePlay.Infrastructure;
using UnityEngine;

namespace GamePlay.Bussiness.Render
{
    public class GameVFXFactory
    {
        public GameVFXFactory()
        {
        }

        public GameVFXEntity Load(string prefabUrl)
        {
            var prefab = GameResourceManager.Load<GameObject>(prefabUrl);
            if (prefab == null)
            {
                GameLogger.LogError($"VFX加载失败：{prefabUrl}");
                return null;
            }
            var body = GameObject.Instantiate(prefab);
            var root = new GameObject();
            body.transform.SetParent(root.transform);
            body.name = "Body";
            var entity = new GameVFXEntity(root, body, prefabUrl);
            return entity;
        }
    }
}