using GamePlay.Core;
using UnityEngine;

namespace GamePlay.Bussiness.Renderer
{
    public class GameVFXDomainR : GameVFXDomainApiR
    {
        GameContextR _context;
        GameVFXContextR _vfxContext => _context.vfxContext;

        public GameVFXDomainR()
        {
        }

        public void Inject(GameContextR context)
        {
            this._context = context;
        }

        public void Destroy()
        {
            this._vfxContext.repo.ForeachEntities((vfx) =>
            {
                vfx.Destroy();
            }, true);
        }

        public void Tick(float dt)
        {
            var repo = this._vfxContext.repo;
            repo.ForeachEntities((GameVFXEntityR vfx) =>
            {
                vfx.Tick(dt);
                if (!vfx.isPlaying) this._context.cmdBufferService.AddDelayCmd(0, () => repo.Recycle(vfx));
                var curField = this._context.fieldContext.curField;
                this._UpdateLayerOrder(vfx);
            });
        }

        private void _UpdateLayerOrder(GameVFXEntityR vfxEntity)
        {
            var trans = vfxEntity.root.transform;
            trans.TryGetSortingLayer(out var order, out var layerName);
            var layerType = vfxEntity.playArgs.layerType;

            if (layerType == GameFieldLayerType.None) layerType = GameFieldLayerType.VFX;//默认为VFX层

            var newOrder = GameFieldLayerCollection.GetLayerOrder(layerType, trans.position);
            if (layerType == GameFieldLayerType.Entity) newOrder += 1;//若为实体层，则在显示在实体上方

            if (order == newOrder) return;
            trans.SetSortingLayer(newOrder, layerName);
        }

        public GameVFXEntityR Play(in GameVFXPlayArgs args)
        {
            var repo = this._vfxContext.repo;
            var factory = this._vfxContext.factory;
            var prefabUrl = args.url;
            if (!repo.TryFetch(prefabUrl, out GameVFXEntityR vfx))
            {
                vfx = factory.Load(prefabUrl);
                if (vfx == null)
                {
                    GameLogger.LogError("VFX加载失败");
                    return null;
                }
                var layerType = args.layerType;
                if (layerType == GameFieldLayerType.None) layerType = GameFieldLayerType.VFX;//默认为VFX层
                this._context.domainApi.fielApi.AddToLayer(vfx.root, layerType);
            }
            vfx.entityId = this._vfxContext.entityIdService.FetchId();
            repo.TryAdd(vfx);
            vfx.Play(args);
            return vfx;
        }
    }
}