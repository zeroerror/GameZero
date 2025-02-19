using GamePlay.Core;
using GamePlay.Infrastructure;
using UnityEngine;

namespace GamePlay.Bussiness.UI
{
    public class UIFactory
    {
        public GameObject LoadUI(string prefabUrl)
        {
            var prefab = GameResourceManager.Load<GameObject>(prefabUrl);
            if (!prefab)
            {
                GameLogger.LogError($"UI工厂: 加载UI预制体失败 {prefabUrl}");
                return null;
            }
            var uiGO = GameObject.Instantiate(prefab);
            return uiGO;
        }
    }
}