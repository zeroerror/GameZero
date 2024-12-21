using GamePlay.Core;

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
                if (!vfx.isPlaying) this._context.cmdBufferService.Add(0, () => repo.Recycle(vfx));
            });
        }

        public GameVFXEntityR Play(in GameVFXPlayArgs args)
        {
            var repo = this._vfxContext.repo;
            var factory = this._vfxContext.factory;
            var prefabUrl = args.prefabUrl;
            if (!repo.TryFetch(prefabUrl, out GameVFXEntityR vfx))
            {
                vfx = factory.Load(prefabUrl);
                if (vfx == null)
                {
                    GameLogger.LogError("VFX加载失败");
                    return null;
                }
                this._context.domainApi.fielApi.AddToLayer(vfx.go, GameFieldLayerType.VFX);
            }
            vfx.entityId = this._vfxContext.entityIdService.FetchId();
            repo.TryAdd(vfx);
            vfx.Play(args);
            return vfx;
        }
    }
}