using GamePlay.Core;
using UnityEngine;

namespace GamePlay.Bussiness.Renderer
{
    public class GameFieldFactoryR
    {
        public GameFieldTemplateR template { get; private set; }
        public GameObject sceneRoot { get; private set; }

        public GameFieldFactoryR(GameObject sceneRoot)
        {
            this.sceneRoot = sceneRoot;
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
            var go = GameObject.Instantiate(model.fieldPrefab);
            go.name = "Field_" + typeId;
            go.transform.SetParent(sceneRoot.transform);

            // 背景层级
            var backgroundLayer = go.transform.Find(GameFieldLayerCollection.BackgroundLayer)?.gameObject;
            if (backgroundLayer == null)
            {
                backgroundLayer = new GameObject(GameFieldLayerCollection.BackgroundLayer);
                backgroundLayer.transform.SetParent(go.transform);
            }

            // 地面层级
            var groundLayer = go.transform.Find(GameFieldLayerCollection.GroundLayer)?.gameObject;
            if (groundLayer == null)
            {
                groundLayer = new GameObject(GameFieldLayerCollection.GroundLayer);
                groundLayer.transform.SetParent(go.transform);
            }

            // 环境层级
            var environmentLayer = go.transform.Find(GameFieldLayerCollection.EnvironmentLayer)?.gameObject;
            if (environmentLayer == null)
            {
                environmentLayer = new GameObject(GameFieldLayerCollection.EnvironmentLayer);
                environmentLayer.transform.SetParent(go.transform);
            }
            for (var i = 0; i < environmentLayer.transform.childCount; i++)
            {
                var child = environmentLayer.transform.GetChild(i);
                if (child.gameObject.name == GameFieldLayerCollection.GroundLayer) continue;
                if (child.gameObject.name == GameFieldLayerCollection.BackgroundLayer) continue;
                if (child.gameObject.name == GameFieldLayerCollection.EnvironmentLayer) continue;
                if (child.gameObject.name == GameFieldLayerCollection.EntityLayer) continue;
                child.SetParent(environmentLayer.transform);
                // 层级关系
                var pos = child.position;
                pos.z = pos.y;
                child.position = pos;
            }

            // 实体层级
            var entityLayer = go.transform.Find(GameFieldLayerCollection.EntityLayer)?.gameObject;
            if (entityLayer == null)
            {
                entityLayer = new GameObject(GameFieldLayerCollection.EntityLayer);
                entityLayer.transform.SetParent(go.transform);
            }

            var entity = new GameFieldEntityR(model, go, entityLayer, environmentLayer, groundLayer);
            return entity;
        }
    }
}