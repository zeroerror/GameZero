
using GamePlay.Bussiness.Logic;
using GamePlay.Core;
using GameVec2 = UnityEngine.Vector2;

namespace GamePlay.Bussiness.Renderer
{
    public class GameActionDomainR : GameActionDomainApiR
    {
        GameContextR _context;
        GameActionContextR _actionContext => _context.actionContext;

        public GameActionDomainR()
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
            this._context.BindRC(GameActionRCCollection.RC_GAME_ACTION_DO, this._OnActionDo);
        }

        private void _UnbindEvents()
        {
            this._context.UnbindRC(GameActionRCCollection.RC_GAME_ACTION_DO, this._OnActionDo);
        }

        private void _OnActionDo(object args)
        {
            var evArgs = (GameActionRCArgs_Do)args;
            var actorEntity = this._context.FindEntity(evArgs.actorIdArgs);
            var targetEntity = this._context.FindEntity(evArgs.targetIdArgs);
            if (actorEntity == null || targetEntity == null)
            {
                this._context.DelayRC(GameActionRCCollection.RC_GAME_ACTION_DO, args);
                return;
            }
            this._DoAction(evArgs.actionId, actorEntity, targetEntity);
        }

        private void _DoAction(int actionId, GameEntityBase actorEntity, GameEntityBase targetEntity)
        {
            if (!this._actionContext.template.TryGet(actionId, out var action))
            {
                GameLogger.LogError("行为执行: 未知的行为Id " + actionId);
                return;
            }
            // 特效
            if (action.vfxPrefabUrl != null)
            {

                var attachNode = targetEntity is GameRoleEntityR targetEntityR ? targetEntityR.go : null;
                var attachOffset = action.vfxOffset;
                attachOffset.x = targetEntity.transformCom.forward.x < 0 ? -attachOffset.x : attachOffset.x;
                var attachPos = targetEntity.transformCom.position + attachOffset;
                var args = new GameVFXPlayArgs()
                {
                    attachNode = attachNode,
                    attachOffset = attachOffset,
                    position = attachPos,
                    prefabUrl = action.vfxPrefabUrl,
                    angle = targetEntity.transformCom.angle,
                    scale = action.vfxScale,
                    loopDuration = 0,
                };
                this._context.domainApi.vfxApi.Play(args);
            }
            // 震屏
            var shakeModel = action.shakeModel;
            if (shakeModel != null)
            {
                this._context.cameraEntity.shakeCom.Shake(shakeModel);
            }
        }
    }
}