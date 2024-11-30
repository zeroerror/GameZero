using System;
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

        public void Dispose()
        {
        }

        public void Tick(float dt)
        {
            var repo = this._vfxContext.repo;
            repo.ForeachEntities((GameVFXEntityR vfx) =>
            {
                vfx.Tick(dt);
                if (!vfx.isPlaying) this._context.cmdBufferService.Add(0, () => repo.TryRemove(vfx));
            });
        }

        public GameVFXEntityR Play(in GameVFXPlayArgs args)
        {
            var repo = this._vfxContext.repo;
            if (!repo.TryFetch(out GameVFXEntityR vfx))
            {
                var factory = this._vfxContext.factory;
                vfx = factory.Load();
            }
            vfx.entityId = this._vfxContext.entityIdService.FetchId();
            repo.TryAdd(vfx);
            vfx.Play(args);
            return vfx;
        }
    }
}