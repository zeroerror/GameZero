using GamePlay.Core;
using UnityEngine;

namespace GamePlay.Bussiness.Renderer
{
    public class GameFieldFactoryR
    {
        public GameFieldTemplateR template { get; private set; }
        public GameFieldFactoryR()
        {
            template = new GameFieldTemplateR();
        }
        public GameFieldEntityR Load(int typeId)
        {
            if (!template.TryGet(typeId, out var model))
            {
                GameLogger.LogError("GameFieldFactoryR.Load: typeId not found: " + typeId);
                return null;
            }
            // 场景根节点 TODO 根据model的预制体实例化
            var go = GameObject.Find("Field");

            // 实体层级
            var entityLayer = new GameObject(GameFieldLayerCollection.EntityLayer);
            entityLayer.transform.SetParent(go.transform);

            // 环境层级
            var environmentLayer = new GameObject(GameFieldLayerCollection.EnvironmentLayer);
            environmentLayer.transform.SetParent(go.transform);
            for (var i = 0; i < environmentLayer.transform.childCount; i++)
            {
                var child = environmentLayer.transform.GetChild(i);
                if (child.gameObject.name == GameFieldLayerCollection.EntityLayer) continue;
                if (child.gameObject.name == GameFieldLayerCollection.EnvironmentLayer) continue;
                child.SetParent(environmentLayer.transform);
                // 层级关系
                var pos = child.position;
                pos.z = pos.y;
                child.position = pos;
            }

            // 地面层级
            var groundLayer = new GameObject(GameFieldLayerCollection.GroundLayer);
            groundLayer.transform.SetParent(go.transform);

            var entity = new GameFieldEntityR(model, go, entityLayer, environmentLayer, groundLayer);
            return entity;
        }
    }
}