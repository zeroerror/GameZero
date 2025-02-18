using GamePlay.Bussiness.Logic;
using GamePlay.Core;
using GamePlay.Infrastructure;
using UnityEngine;
using UnityEngine.Rendering;

namespace GamePlay.Bussiness.Render
{
    public class GameFieldDomainR : GameFieldDomainApiR
    {
        GameContextR _context;
        GameFieldContextR _fieldContext => this._context.fieldContext;

        public GameFieldDomainR()
        {
        }

        public void Inject(GameContextR context)
        {
            this._context = context;
            context.cmdBufferService.AddIntervalCmd(0.1f, this._UpdateLayerOrder);
        }

        public void Destroy()
        {
        }

        public void BindEvents()
        {
            this._context.BindRC(GameFieldRCCollection.RC_GAME_FIELD_CREATE, this._OnCreateField);
        }

        public void UnbindEvents()
        {
        }

        public void Tick(float dt)
        {
        }

        private void _UpdateLayerOrder()
        {
            var curField = this._fieldContext.curField;
            if (!curField) return;
            this._UpdateLayerOrder(curField.GetLayer(GameFieldLayerType.Entity), GameFieldLayerType.Entity);
        }

        private void _UpdateLayerOrder(GameObject layer, GameFieldLayerType layerType)
        {
            foreach (Transform trans in layer.transform)
            {
                trans.TryGetSortingLayer(out var order, out var layerName);
                var newOrder = GameFieldLayerCollection.GetLayerOrder(layerType, trans.position);
                if (order == newOrder) continue;
                trans.SetSortingOrder(newOrder, layerName);
            }
        }

        private void _OnCreateField(object args)
        {
            var rcArgs = (GameFieldRCArgs_Create)args;
            var fieldId = rcArgs.typeId;
            this.LoadField(fieldId);
        }

        public void LoadField(int fieldId)
        {
            var repo = this._fieldContext.repo;
            if (!repo.TryFetch(fieldId, out var field)) field = this._fieldContext.factory.Load(fieldId);
            if (field == null)
            {
                GameLogger.LogError("场景加载失败, 没有找到场景模板: " + fieldId);
                return;
            }
            this._fieldContext.curField = field;
            var cameraEntity = this._context.cameraEntity;
            cameraEntity.SetPosZ(GameFieldLayerCollection.CAMERA_Z);
        }

        public void AddToLayer(GameObject go, GameFieldLayerType layerType, int orderOffset = 0)
        {
            if (!go) return;
            var curField = this._fieldContext.curField;
            var entityLayer = curField.GetLayer(layerType);
            go.transform.SetParent(entityLayer.transform);
            go.SetPosZ(0);

            var sortingLayerName = layerType.ToString();
            var order = GameFieldLayerCollection.GetLayerOrder(layerType, go.transform.position);
            order += orderOffset;

            if (go.TryGetComponent<UnityEngine.Renderer>(out var renderer))
            {
                renderer.sortingLayerName = sortingLayerName;
                renderer.sortingOrder = order;
                return;
            }

            if (!go.TryGetComponent<SortingGroup>(out var sortingGroup))
            {
                sortingGroup = go.AddComponent<SortingGroup>();
            }
            sortingGroup.sortingOrder = order;
            sortingGroup.sortingLayerName = sortingLayerName;
        }
    }
}