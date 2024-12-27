
using System.Runtime.InteropServices;
using GamePlay.Bussiness.Logic;
using GamePlay.Core;
using UnityEngine;
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

        public void Destroy()
        {
            this._UnbindEvents();
        }

        private void _BindEvent()
        {

            this._context.BindRC(GameActionRCCollection.RC_GAME_ACTION_DO, this._OnAction_Do);
            this._context.BindRC(GameActionRCCollection.RC_GAME_ACTION_DMG, this._OnAction_Dmg);
            this._context.BindRC(GameActionRCCollection.RC_GAME_ACTION_HEAL, this._OnAction_Heal);
            this._context.BindRC(GameActionRCCollection.RC_GAME_ACTION_LAUNCH_PROJECTILE, this._OnAction_LaunchProjectile);
            this._context.BindRC(GameActionRCCollection.RC_GAME_ACTION_KNOCK_BACK, this._OnAction_KnockBack);
            this._context.BindRC(GameActionRCCollection.RC_GAME_ACTION_ATTRIBUTE_MODIFY, this._OnAction_AttributeModify);
            this._context.BindRC(GameActionRCCollection.RC_GAME_ACTION_ATTACH_BUFF, this._OnAction_AttachBuff);
        }

        private void _UnbindEvents()
        {
            this._context.UnbindRC(GameActionRCCollection.RC_GAME_ACTION_DO, this._OnAction_Do);
            this._context.UnbindRC(GameActionRCCollection.RC_GAME_ACTION_DMG, this._OnAction_Dmg);
            this._context.UnbindRC(GameActionRCCollection.RC_GAME_ACTION_HEAL, this._OnAction_Heal);
            this._context.UnbindRC(GameActionRCCollection.RC_GAME_ACTION_LAUNCH_PROJECTILE, this._OnAction_LaunchProjectile);
            this._context.UnbindRC(GameActionRCCollection.RC_GAME_ACTION_KNOCK_BACK, this._OnAction_KnockBack);
            this._context.UnbindRC(GameActionRCCollection.RC_GAME_ACTION_ATTRIBUTE_MODIFY, this._OnAction_AttributeModify);
            this._context.UnbindRC(GameActionRCCollection.RC_GAME_ACTION_ATTACH_BUFF, this._OnAction_AttachBuff);
        }

        /// <summary>
        /// 播放行为特效
        /// </summary>
        /// <param name="actionId"></param>
        /// <param name="actor"></param>
        /// <param name="target"></param>
        private void _PlayActionEffect(int actionId, GameEntityBase actor)
        {
            if (!this._actionContext.template.TryGet(actionId, out var action))
            {
                GameLogger.LogError("行为执行: 未知的行为Id " + actionId);
                return;
            }
            // 特效
            var actEffectUrl = action.actEffectUrl;
            if (actEffectUrl != null)
            {
                var attachNode = actor is GameRoleEntityR actRole ? actRole.root : null;
                var transCom = actor.transformCom;
                var attachOffset = action.actVFXOffset;
                attachOffset.x = transCom.forward.x < 0 ? -attachOffset.x : attachOffset.x;
                var attachPos = transCom.position + attachOffset;
                var args = new GameVFXPlayArgs()
                {
                    attachNode = attachNode,
                    attachOffset = attachOffset,
                    position = attachPos,
                    url = actEffectUrl,
                    angle = transCom.angle,
                    scale = action.actVFXScale,
                    loopDuration = 0,
                };
                this._context.domainApi.vfxApi.Play(args);
            }
            // 震屏
            if (action.actCamShakeModel != null)
            {
                this._context.cameraEntity.shakeCom.Shake(action.actCamShakeModel);
            }
        }

        /// <summary>
        /// 播放行为命中特效
        /// <para>target: 目标实体</para>
        /// </summary>
        private void _DoActionHitEffect(int actionId, GameEntityBase target)
        {
            if (!this._actionContext.template.TryGet(actionId, out var action))
            {
                GameLogger.LogError("行为执行: 未知的行为Id " + actionId);
                return;
            }
            // 特效
            var hitEffectUrl = action.hitEffectUrl;
            if (hitEffectUrl != null)
            {

                var attachNode = target is GameRoleEntityR targetRole ? targetRole.root : null;
                var transCom = target.transformCom;
                var attachOffset = action.hitVFXOffset;
                attachOffset.x = transCom.forward.x < 0 ? -attachOffset.x : attachOffset.x;
                var attachPos = transCom.position + attachOffset;
                var args = new GameVFXPlayArgs()
                {
                    attachNode = attachNode,
                    attachOffset = attachOffset,
                    position = attachPos,
                    url = hitEffectUrl,
                    angle = transCom.angle,
                    scale = action.hitVFXScale,
                    loopDuration = 0,
                };
                this._context.domainApi.vfxApi.Play(args);
            }
            // 震屏
            if (action.hitCamShakeModel != null)
            {
                this._context.cameraEntity.shakeCom.Shake(action.hitCamShakeModel);
            }
        }

        private void _OnAction_Do(object args)
        {
            var evArgs = (GameActionRCArgs_Do)args;
            ref var actorIdArgs = ref evArgs.actorIdArgs;
            var actor = this._context.FindEntity(actorIdArgs);
            if (actor == null)
            {
                this._context.DelayRC(GameActionRCCollection.RC_GAME_ACTION_DO, args);
                return;
            }
            this._PlayActionEffect(evArgs.actionId, actor);
        }

        private void _OnAction_Dmg(object args)
        {
            var evArgs = (GameActionRCArgs_Dmg)args;
            ref var dmgRecord = ref evArgs.dmgRecord;
            ref var actorIdArgs = ref dmgRecord.actorIdArgs;
            var actor = this._context.FindEntity(actorIdArgs);
            ref var targetIdArgs = ref dmgRecord.targetIdArgs;
            var target = this._context.FindEntity(targetIdArgs);
            if (actor == null || target == null)
            {
                this._context.DelayRC(GameActionRCCollection.RC_GAME_ACTION_DMG, args);
                return;
            }

            if (evArgs.dmgRecord.value > 0)
            {
                this._DoActionHitEffect(evArgs.actionId, target);
            }

            // 伤害跳字
            var jumpTextDomainApi = this._context.uiContext.domainApi.jumpTextDomainApi;
            var randomStyle = GameMath.RandomRange(1, 5);//1-4
            var jumpPos = this.WorldToScreenPoint(target.transformCom.position);
            jumpPos.y += 50;
            jumpTextDomainApi.JumpText_Dmg(jumpPos, randomStyle, dmgRecord.value.ToString(), 0.5f);
        }

        public Vector3 WorldToScreenPoint(in Vector3 v)
        {
            var pos = RectTransformUtility.WorldToScreenPoint(this._context.cameraEntity.camera, v);
            pos.x -= Screen.width / 2;
            pos.y -= Screen.height / 2;
            return pos;
        }

        private void _OnAction_Heal(object args)
        {
            var evArgs = (GameActionRCArgs_Heal)args;
            ref var healRecord = ref evArgs.healRecord;
            ref var actorIdArgs = ref healRecord.actorIdArgs;
            var actor = this._context.FindEntity(actorIdArgs);
            ref var targetIdArgs = ref healRecord.targetIdArgs;
            var target = this._context.FindEntity(targetIdArgs);
            if (actor == null || target == null)
            {
                this._context.DelayRC(GameActionRCCollection.RC_GAME_ACTION_HEAL, args);
                return;
            }
            if (healRecord.value > 0)
            {
                this._DoActionHitEffect(evArgs.actionId, target);
            }
        }

        private void _OnAction_LaunchProjectile(object args)
        {
            var evArgs = (GameActionRCArgs_LaunchProjectile)args;
            ref var record = ref evArgs.record;
            ref var actorIdArgs = ref record.actorIdArgs;
            var actor = this._context.FindEntity(actorIdArgs);
            ref var targetIdArgs = ref record.targetIdArgs;
            var target = this._context.FindEntity(targetIdArgs);
            if (actor == null || target == null)
            {
                this._context.DelayRC(GameActionRCCollection.RC_GAME_ACTION_LAUNCH_PROJECTILE, args);
                return;
            }
            this._DoActionHitEffect(evArgs.actionId, target);
        }

        private void _OnAction_KnockBack(object args)
        {
            var evArgs = (GameActionRCArgs_KnockBack)args;
            ref var record = ref evArgs.record;
            ref var actorIdArgs = ref record.actorIdArgs;
            var actor = this._context.FindEntity(actorIdArgs);
            ref var targetIdArgs = ref record.targetIdArgs;
            var target = this._context.FindEntity(targetIdArgs);
            if (actor == null || target == null)
            {
                this._context.DelayRC(GameActionRCCollection.RC_GAME_ACTION_KNOCK_BACK, args);
                return;
            }
            this._DoActionHitEffect(evArgs.actionId, target);
        }

        private void _OnAction_AttributeModify(object args)
        {
            var evArgs = (GameActionRCArgs_AttributeModify)args;
            ref var record = ref evArgs.record;
            ref var actorIdArgs = ref record.actorIdArgs;
            var actor = this._context.FindEntity(actorIdArgs);
            ref var targetIdArgs = ref record.targetIdArgs;
            var target = this._context.FindEntity(targetIdArgs);
            if (actor == null || target == null)
            {
                this._context.DelayRC(GameActionRCCollection.RC_GAME_ACTION_ATTRIBUTE_MODIFY, args);
                return;
            }
            this._DoActionHitEffect(evArgs.actionId, target);
        }

        private void _OnAction_AttachBuff(object args)
        {
            var evArgs = (GameActionRCArgs_AttachBuff)args;
            ref var record = ref evArgs.record;
            ref var actorIdArgs = ref record.actorIdArgs;
            var actor = this._context.FindEntity(actorIdArgs);
            ref var targetIdArgs = ref record.targetIdArgs;
            var target = this._context.FindEntity(targetIdArgs);
            if (actor == null || target == null)
            {
                this._context.DelayRC(GameActionRCCollection.RC_GAME_ACTION_ATTACH_BUFF, args);
                return;
            }
            this._DoActionHitEffect(evArgs.actionId, target);
        }
    }
}