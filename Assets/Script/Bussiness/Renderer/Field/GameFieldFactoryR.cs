using System.Collections.Generic;
using GamePlay.Core;
using Unity.VisualScripting;
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
            // 场景根节点 
            var go = GameObject.Instantiate(model.fieldPrefab);
            go.name = "Field_" + typeId;
            go.transform.SetParent(sceneRoot.transform);

            Dictionary<GameFieldLayerType, GameObject> layerDict = new Dictionary<GameFieldLayerType, GameObject>();

            // 背景层级
            var backgroundLayer = go.transform.Find(GameFieldLayerCollection.BackgroundLayer)?.gameObject;
            if (!backgroundLayer) backgroundLayer = new GameObject(GameFieldLayerCollection.BackgroundLayer);
            backgroundLayer.transform.SetParent(go.transform);
            backgroundLayer.transform.position = new Vector3(0, 0, GameFieldLayerCollection.BackgroundLayerZ);
            layerDict.Add(GameFieldLayerType.Background, backgroundLayer);

            // 地面层级
            var groundLayer = go.transform.Find(GameFieldLayerCollection.GroundLayer)?.gameObject;
            if (!groundLayer) groundLayer = new GameObject(GameFieldLayerCollection.GroundLayer);
            groundLayer.transform.SetParent(go.transform);
            groundLayer.transform.position = new Vector3(0, 0, GameFieldLayerCollection.GroundLayerZ);
            layerDict.Add(GameFieldLayerType.Ground, groundLayer);
            // 地面层级子节点 
            for (var i = 0; i < groundLayer.transform.childCount; i++)
            {
                var child = groundLayer.transform.GetChild(i);
                if (child.gameObject.name == GameFieldLayerCollection.GroundLayer) continue;
                if (child.gameObject.name == GameFieldLayerCollection.BackgroundLayer) continue;
                if (child.gameObject.name == GameFieldLayerCollection.EnvironmentLayer) continue;
                if (child.gameObject.name == GameFieldLayerCollection.EntityLayer) continue;
                child.SetParent(groundLayer.transform);
                // 层级关系
                var pos = child.position;
                pos.z = GameFieldLayerCollection.GroundLayerZ + pos.y * GameFieldLayerCollection.StepZ;
                child.position = pos;
            }

            // 环境层级
            var environmentLayer = go.transform.Find(GameFieldLayerCollection.EnvironmentLayer)?.gameObject;
            if (!environmentLayer) environmentLayer = new GameObject(GameFieldLayerCollection.EnvironmentLayer);
            environmentLayer.transform.SetParent(go.transform);
            environmentLayer.transform.position = new Vector3(0, 0, GameFieldLayerCollection.EnvironmentLayerZ);
            layerDict.Add(GameFieldLayerType.Environment, environmentLayer);
            // 环境层级子节点
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
                pos.z = GameFieldLayerCollection.EnvironmentLayerZ + pos.y * GameFieldLayerCollection.StepZ;
                child.position = pos;
            }

            // 实体层级
            var entityLayer = go.transform.Find(GameFieldLayerCollection.EntityLayer)?.gameObject;
            if (!entityLayer) entityLayer = new GameObject(GameFieldLayerCollection.EntityLayer);
            entityLayer.transform.SetParent(go.transform);
            entityLayer.transform.position = new Vector3(0, 0, GameFieldLayerCollection.EntityLayerZ);
            layerDict.Add(GameFieldLayerType.Entity, entityLayer);

            // VFX层级
            var vfxLayer = go.transform.Find(GameFieldLayerCollection.VFXLayer)?.gameObject;
            if (!vfxLayer) vfxLayer = new GameObject(GameFieldLayerCollection.VFXLayer);
            vfxLayer.transform.SetParent(go.transform);
            vfxLayer.transform.position = new Vector3(0, 0, GameFieldLayerCollection.VFXLayerZ);
            layerDict.Add(GameFieldLayerType.VFX, vfxLayer);

            // 场景UI层级
            var sceneUILayer = go.transform.Find(GameFieldLayerCollection.SceneUILayer)?.gameObject;
            if (!sceneUILayer) sceneUILayer = new GameObject(GameFieldLayerCollection.SceneUILayer);
            sceneUILayer.transform.SetParent(go.transform);
            sceneUILayer.transform.position = new Vector3(0, 0, GameFieldLayerCollection.SceneUILayerZ);
            layerDict.Add(GameFieldLayerType.SceneUI, sceneUILayer);

            var entity = new GameFieldEntityR(model, go, layerDict);
            return entity;
        }
    }
}