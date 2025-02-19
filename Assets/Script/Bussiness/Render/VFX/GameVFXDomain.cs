using GamePlay.Core;
using GamePlay.Infrastructure;

namespace GamePlay.Bussiness.Render
{
    public class GameVFXDomain : GameVFXDomainApi
    {
        GameContextR _context;
        GameVFXContext _vfxContext => _context.vfxContext;

        public GameVFXDomain()
        {
        }

        public void Inject(GameContextR context)
        {
            this._context = context;
        }

        public void BindEvents()
        {
        }

        public void UnbindEvents()
        {
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
            repo.ForeachEntities((GameVFXEntity vfx) =>
            {
                vfx.Tick(dt);
                if (!vfx.isPlaying) this.Stop(vfx);
            });
        }

        public GameVFXEntity Play(in GameVFXPlayArgs args)
        {
            var repo = this._vfxContext.repo;
            var factory = this._vfxContext.factory;
            var prefabUrl = args.url;
            if (!repo.TryFetch(prefabUrl, out GameVFXEntity vfx))
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

        public void Stop(GameVFXEntity vfxEntity)
        {
            if (vfxEntity == null) return;
            vfxEntity.Stop();
            var repo = this._vfxContext.repo;
            this._context.cmdBufferService.AddDelayCmd(0, () => repo.Recycle(vfxEntity));
        }
    }
}