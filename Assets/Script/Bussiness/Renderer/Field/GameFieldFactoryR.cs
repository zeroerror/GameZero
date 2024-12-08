using System.Collections.Generic;
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
            // 场景根节点 
            var go = GameObject.Instantiate(model.fieldPrefab);
            go.name = "Field_" + typeId;
            go.transform.SetParent(sceneRoot.transform);

            Dictionary<GameFieldLayerType, GameObject> layerDict = new Dictionary<GameFieldLayerType, GameObject>();

            // 背景层级
            var backgroundLayer = go.transform.Find(GameFieldLayerCollection.BackgroundLayer)?.gameObject;
            if (!backgroundLayer) backgroundLayer = new GameObject(GameFieldLayerCollection.BackgroundLayer);
            backgroundLayer.transform.SetParent(go.transform);
            backgroundLayer.SetPosZ(0);
            backgroundLayer.SetSortingLayer(GameFieldLayerCollection.BackgroundLayerZ, GameFieldLayerCollection.BackgroundLayer);
            layerDict.Add(GameFieldLayerType.Background, backgroundLayer);

            // 地面层级
            var groundLayer = go.transform.Find(GameFieldLayerCollection.GroundLayer)?.gameObject;
            if (!groundLayer) groundLayer = new GameObject(GameFieldLayerCollection.GroundLayer);
            groundLayer.transform.SetParent(go.transform);
            groundLayer.SetPosZ(0);
            groundLayer.SetSortingLayer(GameFieldLayerCollection.GroundLayerZ, GameFieldLayerCollection.GroundLayer);
            layerDict.Add(GameFieldLayerType.Ground, groundLayer);
            // 地面层级子节点 
            groundLayer.ForeachChild((child) =>
            {
                child.SetPosZ(0);
                var order = GameFieldLayerCollection.GetLayerOrder(GameFieldLayerType.Ground, child.transform.position);
                child.SetSortingLayer(order, GameFieldLayerCollection.GroundLayer);
            });

            // 环境层级
            var environmentLayer = go.transform.Find(GameFieldLayerCollection.EnvironmentLayer)?.gameObject;
            if (!environmentLayer) environmentLayer = new GameObject(GameFieldLayerCollection.EnvironmentLayer);
            environmentLayer.transform.SetParent(go.transform);
            environmentLayer.SetPosZ(0);
            environmentLayer.SetSortingLayer(GameFieldLayerCollection.EnvironmentLayerZ, GameFieldLayerCollection.EnvironmentLayer);
            layerDict.Add(GameFieldLayerType.Environment, environmentLayer);
            // 环境层级子节点
            environmentLayer.ForeachChild((child) =>
            {
                child.SetPosZ(0);
                var order = GameFieldLayerCollection.GetLayerOrder(GameFieldLayerType.Environment, child.transform.position);
                child.SetSortingLayer(order, GameFieldLayerCollection.EnvironmentLayer);
            });

            // 实体层级
            var entityLayer = go.transform.Find(GameFieldLayerCollection.EntityLayer)?.gameObject;
            if (!entityLayer) entityLayer = new GameObject(GameFieldLayerCollection.EntityLayer);
            entityLayer.transform.SetParent(go.transform);
            entityLayer.SetPosZ(0);
            entityLayer.SetSortingLayer(GameFieldLayerCollection.EntityLayerZ, GameFieldLayerCollection.EntityLayer);
            layerDict.Add(GameFieldLayerType.Entity, entityLayer);

            // VFX层级
            var vfxLayer = go.transform.Find(GameFieldLayerCollection.VFXLayer)?.gameObject;
            if (!vfxLayer) vfxLayer = new GameObject(GameFieldLayerCollection.VFXLayer);
            vfxLayer.transform.SetParent(go.transform);
            vfxLayer.SetPosZ(0);
            vfxLayer.SetSortingLayer(GameFieldLayerCollection.VFXLayerZ, GameFieldLayerCollection.VFXLayer);
            layerDict.Add(GameFieldLayerType.VFX, vfxLayer);

            // 场景UI层级
            var sceneUILayer = go.transform.Find(GameFieldLayerCollection.SceneUILayer)?.gameObject;
            if (!sceneUILayer) sceneUILayer = new GameObject(GameFieldLayerCollection.SceneUILayer);
            sceneUILayer.transform.SetParent(go.transform);
            sceneUILayer.SetPosZ(0);
            sceneUILayer.SetSortingLayer(GameFieldLayerCollection.SceneUILayerZ, GameFieldLayerCollection.SceneUILayer);
            layerDict.Add(GameFieldLayerType.SceneUI, sceneUILayer);

            var entity = new GameFieldEntityR(model, go, layerDict);
            return entity;
        }
    }
}