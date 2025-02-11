using GamePlay.Core;
using UnityEngine;

namespace GamePlay.Bussiness.Render
{
    public class GameShaderEffectDomain : GameShaderEffectDomainApi
    {
        GameContextR _context;
        GameShaderEffectContext _shaderEffectContext => this._context.shaderEffectContext;

        public GameShaderEffectDomain()
        {
        }

        public void Inject(GameContextR context)
        {
            this._context = context;
            this._BindEvents();
        }

        public void Destroy()
        {
            this._UnbindEvents();
        }

        private void _BindEvents()
        {
        }

        private void _UnbindEvents()
        {
        }

        public void Tick(float dt)
        {
            this._shaderEffectContext.repo.Foreach((shaderEffectEntity) =>
            {
                shaderEffectEntity.Tick(dt);
            });
            this._shaderEffectContext.repo.Foreach((shaderEffectEntity) =>
            {
                if (!shaderEffectEntity.isPlaying)
                {
                    this._shaderEffectContext.repo.Recycle(shaderEffectEntity);
                }
            });
        }

        public void PlayShaderEffect(int shaderEffectId, Renderer renderer)
        {
            var shaderEffectEntity = this._GetShaderEffectEntity(shaderEffectId);
            if (shaderEffectEntity == null)
            {
                return;
            }
            shaderEffectEntity.Play(renderer);
        }
        public void PlayShaderEffect(int shaderEffectId, Renderer[] renderers)
        {
            var shaderEffectEntity = this._GetShaderEffectEntity(shaderEffectId);
            if (shaderEffectEntity == null)
            {
                return;
            }
            renderers?.Foreach((renderer) =>
            {
                shaderEffectEntity.Play(renderer);
            });
            this._shaderEffectContext.repo.Add(shaderEffectEntity);
        }

        private GameShaderEffectEntity _GetShaderEffectEntity(int shaderEffectId)
        {
            if (!this._shaderEffectContext.repo.TryFetch(shaderEffectId, out var entity))
            {
                entity = this._shaderEffectContext.factory.Load(shaderEffectId);
            }
            if (entity == null)
            {
                GameLogger.LogError($"ShaderEffectEntity not found: {shaderEffectId}");
                return null;
            }
            return entity;
        }
    }
}