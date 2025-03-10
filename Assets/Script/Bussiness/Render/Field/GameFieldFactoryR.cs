using System.Collections.Generic;
using GamePlay.Core;
using GamePlay.Infrastructure;
using UnityEngine;

namespace GamePlay.Bussiness.Render
{
    public class GameFieldFactoryR
    {
        public GameFieldTemplateR template { get; private set; }
        public GameObject sceneRoot { get; private set; }

        public GameFieldFactoryR()
        {
            template = new GameFieldTemplateR();
        }

        public void Inject(GameObject sceneRoot)
        {
            this.sceneRoot = sceneRoot;
        }

        public GameFieldEntityR Load(int typeId)
        {
            if (!template.TryGet(typeId, out var model))
            {
                GameLogger.LogError("GameFieldFactoryR.Load: typeId not found: " + typeId);
                return null;
            }
            // 场景根节点 
            var rootGO = GameObject.Instantiate(model.fieldPrefab);
            rootGO.name = "Field_" + typeId;
            rootGO.transform.SetParent(sceneRoot.transform);

            // 背景层级
            Dictionary<GameFieldLayerType, GameObject> layerDict = new Dictionary<GameFieldLayerType, GameObject>();
            var backgroundLayer = rootGO.transform.Find(GameFieldLayerCollection.BackgroundLayer)?.gameObject;
            if (!backgroundLayer) backgroundLayer = new GameObject(GameFieldLayerCollection.BackgroundLayer);
            backgroundLayer.transform.SetParent(rootGO.transform);
            backgroundLayer.SetPosZ(0);
            backgroundLayer.SetSortingOrder(GameFieldLayerCollection.BackgroundLayerZ, GameFieldLayerCollection.BackgroundLayer);
            layerDict.Add(GameFieldLayerType.Background, backgroundLayer);

            // 地面层级
            var groundLayer = rootGO.transform.Find(GameFieldLayerCollection.GroundLayer)?.gameObject;
            if (!groundLayer) groundLayer = new GameObject(GameFieldLayerCollection.GroundLayer);
            groundLayer.transform.SetParent(rootGO.transform);
            groundLayer.SetPosZ(0);
            groundLayer.SetSortingOrder(GameFieldLayerCollection.GroundLayerZ, GameFieldLayerCollection.GroundLayer);
            layerDict.Add(GameFieldLayerType.Ground, groundLayer);
            // 地面层级子节点 
            groundLayer.ForeachChild((child) =>
            {
                child.SetPosZ(0);
                var order = GameFieldLayerCollection.GetLayerOrder(GameFieldLayerType.Ground, child.transform.position);
                child.SetSortingOrder(order, GameFieldLayerCollection.GroundLayer);
            });

            // 环境层级
            var environmentLayer = rootGO.transform.Find(GameFieldLayerCollection.EnvironmentLayer)?.gameObject;
            if (!environmentLayer) environmentLayer = new GameObject(GameFieldLayerCollection.EnvironmentLayer);
            environmentLayer.transform.SetParent(rootGO.transform);
            environmentLayer.SetPosZ(0);
            environmentLayer.SetSortingOrder(GameFieldLayerCollection.EnvironmentLayerZ, GameFieldLayerCollection.EnvironmentLayer);
            layerDict.Add(GameFieldLayerType.Environment, environmentLayer);
            // 环境层级子节点
            environmentLayer.ForeachChild((child) =>
            {
                child.SetPosZ(0);
                var order = GameFieldLayerCollection.GetLayerOrder(GameFieldLayerType.Environment, child.transform.position);
                child.SetSortingOrder(order, GameFieldLayerCollection.EnvironmentLayer);
            });

            // 实体层级
            var entityLayer = rootGO.transform.Find(GameFieldLayerCollection.EntityLayer)?.gameObject;
            if (!entityLayer) entityLayer = new GameObject(GameFieldLayerCollection.EntityLayer);
            entityLayer.transform.SetParent(rootGO.transform);
            entityLayer.SetPosZ(0);
            entityLayer.SetSortingOrder(GameFieldLayerCollection.EntityLayerZ, GameFieldLayerCollection.EntityLayer);
            layerDict.Add(GameFieldLayerType.Entity, entityLayer);

            // VFX层级
            var vfxLayer = rootGO.transform.Find(GameFieldLayerCollection.VFXLayer)?.gameObject;
            if (!vfxLayer) vfxLayer = new GameObject(GameFieldLayerCollection.VFXLayer);
            vfxLayer.transform.SetParent(rootGO.transform);
            vfxLayer.SetPosZ(0);
            vfxLayer.SetSortingOrder(GameFieldLayerCollection.VFXLayerZ, GameFieldLayerCollection.VFXLayer);
            layerDict.Add(GameFieldLayerType.VFX, vfxLayer);

            // 场景UI层级
            var sceneUILayer = rootGO.transform.Find(GameFieldLayerCollection.SceneUILayer)?.gameObject;
            if (!sceneUILayer) sceneUILayer = new GameObject(GameFieldLayerCollection.SceneUILayer);
            sceneUILayer.transform.SetParent(rootGO.transform);
            sceneUILayer.SetPosZ(0);
            sceneUILayer.SetSortingOrder(GameFieldLayerCollection.SceneUILayerZ, GameFieldLayerCollection.SceneUILayer);
            layerDict.Add(GameFieldLayerType.SceneUI, sceneUILayer);

            var entity = new GameFieldEntityR(model, rootGO, layerDict);
            return entity;
        }
    }
}