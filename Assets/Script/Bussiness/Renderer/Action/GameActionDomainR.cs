
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
            this._context.BindRC(GameActionRCCollection.RC_GAME_ACTION_DO_DMG, this._OnAction_DoDmg);
            this._context.BindRC(GameActionRCCollection.RC_GAME_ACTION_DO_HEAL, this._OnAction_DoHeal);
            this._context.BindRC(GameActionRCCollection.RC_GAME_ACTION_LAUNCH_PROJECTILE, this._OnAction_DoLaunchProjectile);
        }

        private void _UnbindEvents()
        {
            this._context.UnbindRC(GameActionRCCollection.RC_GAME_ACTION_DO_DMG, this._OnAction_DoDmg);
            this._context.UnbindRC(GameActionRCCollection.RC_GAME_ACTION_DO_HEAL, this._OnAction_DoHeal);
            this._context.UnbindRC(GameActionRCCollection.RC_GAME_ACTION_LAUNCH_PROJECTILE, this._OnAction_DoLaunchProjectile);
        }

        private void _OnAction_DoDmg(object args)
        {
            var evArgs = (GameActionRCArgs_DoDmg)args;
            ref var dmgRecord = ref evArgs.dmgRecord;
            ref var actorIdArgs = ref dmgRecord.actorIdArgs;
            var actorEntity = this._context.FindEntity(actorIdArgs);
            ref var targetIdArgs = ref dmgRecord.targetIdArgs;
            var targetEntity = this._context.FindEntity(targetIdArgs);
            if (actorEntity == null || targetEntity == null)
            {
                this._context.DelayRC(GameActionRCCollection.RC_GAME_ACTION_DO_DMG, args);
                return;
            }
            this._DoActionRenderer(evArgs.actionId, actorEntity, targetEntity);
        }

        private void _OnAction_DoHeal(object args)
        {
            var evArgs = (GameActionRCArgs_DoHeal)args;
            ref var healRecord = ref evArgs.healRecord;
            ref var actorIdArgs = ref healRecord.actorIdArgs;
            var actorEntity = this._context.FindEntity(actorIdArgs);
            ref var targetIdArgs = ref healRecord.targetIdArgs;
            var targetEntity = this._context.FindEntity(targetIdArgs);
            if (actorEntity == null || targetEntity == null)
            {
                this._context.DelayRC(GameActionRCCollection.RC_GAME_ACTION_DO_HEAL, args);
                return;
            }
            this._DoActionRenderer(evArgs.actionId, actorEntity, targetEntity);
        }

        private void _OnAction_DoLaunchProjectile(object args)
        {
            var evArgs = (GameActionRCArgs_LaunchProjectile)args;
            ref var record = ref evArgs.record;
            ref var actorIdArgs = ref record.actorIdArgs;
            var actorEntity = this._context.FindEntity(actorIdArgs);
            ref var targetIdArgs = ref record.targetIdArgs;
            var targetEntity = this._context.FindEntity(targetIdArgs);
            if (actorEntity == null || targetEntity == null)
            {
                this._context.DelayRC(GameActionRCCollection.RC_GAME_ACTION_LAUNCH_PROJECTILE, args);
                return;
            }
            this._DoActionRenderer(evArgs.actionId, actorEntity, targetEntity);
        }

        /// <summary>
        /// 执行行为触发的表现
        /// </summary>
        /// <param name="actionId"></param>
        /// <param name="actorEntity"></param>
        /// <param name="targetEntity"></param>
        private void _DoActionRenderer(int actionId, GameEntityBase actorEntity, GameEntityBase targetEntity)
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