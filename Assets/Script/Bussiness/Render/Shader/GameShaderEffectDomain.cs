using GamePlay.Bussiness.Logic;
using GamePlay.Core;
using GamePlay.Infrastructure;
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
        }

        public void Destroy()
        {
        }

        public void BindEvents()
        {
        }

        public void UnbindEvents()
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
            this._shaderEffectContext.repo.Add(shaderEffectEntity);
        }

        public void PlayShaderEffect(int shaderEffectId, Renderer[] renderers)
        {
            renderers?.Foreach((renderer) =>
            {
                this.PlayShaderEffect(shaderEffectId, renderer);
            });
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

        public void PlayShaderEffect(int shaderEffectId, GameEntityBase entity)
        {
            if (entity is GameRoleEntityR role)
            {
                this.PlayShaderEffect(shaderEffectId, role.bodyCom);
                return;
            }
            GameLogger.LogError($"PlayShaderEffect: 不支持的实体类型: {entity.idCom.entityType}");
        }

        public void PlayShaderEffect(GameShaderEffectType effType, GameEntityBase entity)
        {
            this.PlayShaderEffect((int)effType, entity);
        }

        public void PlayShaderEffect(int shaderEffectId, GameRoleBodyCom bodyCom)
        {
            Renderer[] renderers = bodyCom.renderers;
            this.PlayShaderEffect(shaderEffectId, renderers);
        }

        public void StopShaderEffect(Renderer renderer)
        {
            this._shaderEffectContext.repo.Foreach((shaderEffectEntity) =>
            {
                if (shaderEffectEntity.renderer == renderer)
                {
                    shaderEffectEntity.Stop();
                    this._shaderEffectContext.repo.Recycle(shaderEffectEntity);
                }
            });
        }

        public void StopShaderEffects(Renderer[] renderers)
        {
            renderers?.Foreach((renderer) =>
            {
                this.StopShaderEffect(renderer);
            });
        }

        public void StopShaderEffects(GameEntityBase entity)
        {
            Renderer[] renderers;
            if (entity is GameRoleEntityR role)
            {
                renderers = role.bodyCom.renderers;
            }
            else
            {
                GameLogger.LogError($"StopShaderEffect: 不支持的实体类型: {entity.idCom.entityType}");
                return;
            }
            renderers?.Foreach((renderer) =>
            {
                this.StopShaderEffect(renderer);
            });
        }
    }
}