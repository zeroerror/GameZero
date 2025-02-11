using GamePlay.Core;
using UnityEngine;

namespace GamePlay.Bussiness.Render
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
                if (!vfx.isPlaying) this.Stop(vfx);
            });
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

        public void Stop(GameVFXEntityR vfxEntity)
        {
            if (vfxEntity == null) return;
            vfxEntity.Stop();
            var repo = this._vfxContext.repo;
            this._context.cmdBufferService.AddDelayCmd(0, () => repo.Recycle(vfxEntity));
        }
    }
}