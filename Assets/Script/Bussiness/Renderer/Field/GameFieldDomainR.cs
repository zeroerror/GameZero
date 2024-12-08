using GamePlay.Bussiness.Logic;
using GamePlay.Core;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

namespace GamePlay.Bussiness.Renderer
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
            this._BindEvent();
        }

        public void Dispose()
        {
            this._UnbindEvents();
        }

        private void _BindEvent()
        {
            this._context.BindRC(GameFieldRCCollection.RC_GAME_FIELD_CREATE, this._OnCreateField);
        }

        private void _UnbindEvents()
        {
        }

        public void Tick(float dt)
        {
            var entityLayer = this._fieldContext.curField.GetLayer(GameFieldLayerType.Entity);
            var rootOrder = GameFieldLayerCollection.EntityLayerZ;
            foreach (Transform entityTf in entityLayer.transform)
            {
                var sortingGroup = entityTf.GetComponent<SortingGroup>();
                var order = sortingGroup.sortingOrder;
                var layerName = sortingGroup.sortingLayerName;
                var newOrder = GameFieldLayerCollection.GetLayerOrder(GameFieldLayerType.Entity, entityTf.position);
                if (order == newOrder) continue;

                sortingGroup.sortingOrder = newOrder;
                entityTf.SetSortingLayer(newOrder, layerName);

                // 子节点的层级根据父节点的层级偏移调整
                var offset = newOrder - order;
                entityTf.ForeachTransfrom_DFS((tf) =>
                {
                    if (!tf.TryGetSortingLayer(out var order, out var layerName)) return;
                    tf.SetSortingLayer(order + offset, layerName);
                });
            }

            var vfxLayer = this._fieldContext.curField.GetLayer(GameFieldLayerType.VFX);
            for (var i = 0; i < vfxLayer.transform.childCount; i++)
            {
                var child = vfxLayer.transform.GetChild(i);
                var pos = child.position;
                pos.z = GameFieldLayerCollection.VFXLayerZ + pos.y * GameFieldLayerCollection.StepZ;
                child.position = pos;
            }
        }

        private void _OnCreateField(object args)
        {
            var evArgs = (GameFieldRCArgs_Create)args;
            var fieldId = evArgs.typeId;
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
            var cam = this._context.cameraEntity.camera;
            cam.transform.position = new Vector3(0, 0, GameFieldLayerCollection.CAMERA_Z);
            cam.transform.eulerAngles = new Vector3(0, 0, 0);
        }

        public void AddToLayer(GameObject go, GameFieldLayerType layerType)
        {
            if (!go) return;
            var curField = this._fieldContext.curField;
            var entityLayer = curField.GetLayer(layerType);
            go.transform.SetParent(entityLayer.transform);
            go.SetPosZ(0);

            if (layerType == GameFieldLayerType.Entity)
            {
                if (!go.TryGetComponent<SortingGroup>(out var sortingGroup))
                {
                    sortingGroup = go.AddComponent<SortingGroup>();
                }
                var order = GameFieldLayerCollection.GetLayerOrder(layerType, go.transform.position);
                sortingGroup.sortingOrder = order;
            }
            else
            {
                var order = GameFieldLayerCollection.GetLayerOrder(layerType, go.transform.position);
                go.SetSortingLayer(order, layerType.ToString());
            }
        }
    }
}