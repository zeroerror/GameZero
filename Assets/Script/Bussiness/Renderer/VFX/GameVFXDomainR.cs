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
            var newOrder = GameFieldLayerCollection.GetLayerOrder(GameFieldLayerType.VFX, trans.position);
            newOrder += 1;
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
                this._context.domainApi.fielApi.AddToLayer(vfx.root, GameFieldLayerType.VFX);
            }
            vfx.entityId = this._vfxContext.entityIdService.FetchId();
            repo.TryAdd(vfx);
            vfx.Play(args);
            return vfx;
        }
    }
}