
using System.Runtime.InteropServices;
using GamePlay.Bussiness.Logic;
using GamePlay.Core;
using GamePlay.Infrastructure;
using UnityEngine;
using GameVec2 = UnityEngine.Vector2;

namespace GamePlay.Bussiness.Render
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
        }

        public void Destroy()
        {
        }

        public void BindEvents()
        {
            this._context.BindRC(GameActionRCCollection.RC_GAME_ACTION_DO, this._OnAction_Do);
            this._context.BindRC(GameActionRCCollection.RC_GAME_ACTION_DMG, this._OnAction_Dmg);
            this._context.BindRC(GameActionRCCollection.RC_GAME_ACTION_HEAL, this._OnAction_Heal);
            this._context.BindRC(GameActionRCCollection.RC_GAME_ACTION_LAUNCH_PROJECTILE, this._OnAction_LaunchProjectile);
            this._context.BindRC(GameActionRCCollection.RC_GAME_ACTION_KNOCK_BACK, this._OnAction_KnockBack);
            this._context.BindRC(GameActionRCCollection.RC_GAME_ACTION_ATTRIBUTE_MODIFY, this._OnAction_AttributeModify);
            this._context.BindRC(GameActionRCCollection.RC_GAME_ACTION_ATTACH_BUFF, this._OnAction_AttachBuff);
            this._context.BindRC(GameActionRCCollection.RC_GAME_ACTION_TRANSFORM, this._OnAction_CharacterTransform);
            this._context.BindRC(GameActionRCCollection.RC_GAME_ACTION_STEALTH, this._OnAction_Stealth);
        }

        public void UnbindEvents()
        {
            this._context.UnbindRC(GameActionRCCollection.RC_GAME_ACTION_DO, this._OnAction_Do);
            this._context.UnbindRC(GameActionRCCollection.RC_GAME_ACTION_DMG, this._OnAction_Dmg);
            this._context.UnbindRC(GameActionRCCollection.RC_GAME_ACTION_HEAL, this._OnAction_Heal);
            this._context.UnbindRC(GameActionRCCollection.RC_GAME_ACTION_LAUNCH_PROJECTILE, this._OnAction_LaunchProjectile);
            this._context.UnbindRC(GameActionRCCollection.RC_GAME_ACTION_KNOCK_BACK, this._OnAction_KnockBack);
            this._context.UnbindRC(GameActionRCCollection.RC_GAME_ACTION_ATTRIBUTE_MODIFY, this._OnAction_AttributeModify);
            this._context.UnbindRC(GameActionRCCollection.RC_GAME_ACTION_ATTACH_BUFF, this._OnAction_AttachBuff);
            this._context.UnbindRC(GameActionRCCollection.RC_GAME_ACTION_TRANSFORM, this._OnAction_CharacterTransform);
            this._context.UnbindRC(GameActionRCCollection.RC_GAME_ACTION_STEALTH, this._OnAction_Stealth);
        }

        public bool TryGetModel(int actionId, out GameActionModelR model)
        {
            return this._actionContext.template.TryGet(actionId, out model);
        }

        /// <summary>
        /// 播放行为特效
        /// <para>actor: 执行者实体</para>
        /// <para>actPos: 执行位置</para>
        /// </summary>
        private void _PlayActionEffect(int actionId, in Vector2 actPos)
        {
            if (!this._actionContext.template.TryGet(actionId, out var action))
            {
                GameLogger.LogError("行为执行: 未知的行为Id " + actionId);
                return;
            }
            // 视觉特效
            var actVFXUrl = action.actVFXUrl;
            if (actVFXUrl != null)
            {
                var vfxOffset = action.actVFXOffset;
                var vfxPos = actPos + vfxOffset;
                var args = new GameVFXPlayArgs()
                {
                    position = vfxPos,
                    url = actVFXUrl,
                    scale = action.actVFXScale,
                    loopDuration = 0,
                    layerType = GameFieldLayerType.VFX
                };
                this._context.domainApi.vfxApi.Play(args);
            }
            // 音效
            var actSFXUrl = action.actSFXUrl;
            if (actSFXUrl != null)
            {
                var sfx = this._context.audioService.PlaySFX(actSFXUrl);
                sfx.SetVolume(action.actSFXVolume);
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
        private void _PlayActionHitEffect(int actionId, GameEntityBase target)
        {
            if (!this._actionContext.template.TryGet(actionId, out var action))
            {
                GameLogger.LogError("行为执行: 未知的行为Id " + actionId);
                return;
            }
            // 视觉特效
            var hitVFXUrl = action.hitVFXUrl;
            if (hitVFXUrl != null)
            {

                var attachNode = target is GameRoleEntityR targetRole ? targetRole.bodyCom.tmRoot : null;
                var transCom = target.transformCom;
                var attachOffset = action.hitVFXOffset;
                attachOffset.x = transCom.forward.x < 0 ? -attachOffset.x : attachOffset.x;
                var attachPos = transCom.position + attachOffset;
                var args = new GameVFXPlayArgs()
                {
                    attachNode = attachNode,
                    attachOffset = attachOffset,
                    position = attachPos,
                    url = hitVFXUrl,
                    angle = transCom.angle,
                    scale = action.hitVFXScale,
                    loopDuration = 0,
                    layerType = GameFieldLayerType.VFX
                };
                this._context.domainApi.vfxApi.Play(args);
            }
            // 音效
            var hitSFXUrl = action.hitSFXUrl;
            if (hitSFXUrl != null)
            {
                var sfx = this._context.audioService.PlaySFX(hitSFXUrl);
                sfx.SetVolume(action.hitSFXVolume);
            }
            // 震屏
            if (action.hitCamShakeModel != null)
            {
                this._context.cameraEntity.shakeCom.Shake(action.hitCamShakeModel);
            }
        }

        private void _OnAction_Do(object args)
        {
            var rcArgs = (GameActionRCArgs_Do)args;
            ref var actorIdArgs = ref rcArgs.actorIdArgs;
            var actor = this._context.FindEntity(actorIdArgs);
            if (actor == null)
            {
                this._context.DelayRC(GameActionRCCollection.RC_GAME_ACTION_DO, args);
                return;
            }
            this._PlayActionEffect(rcArgs.actionId, rcArgs.actPos);
        }

        private void _OnAction_Dmg(object args)
        {
            var rcArgs = (GameActionRCArgs_Dmg)args;
            ref var dmgRecord = ref rcArgs.dmgRecord;
            ref var actorIdArgs = ref dmgRecord.actorIdArgs;
            var actor = this._context.FindEntity(actorIdArgs);
            ref var targetIdArgs = ref dmgRecord.targetIdArgs;
            var target = this._context.FindEntity(targetIdArgs);
            if (actor == null || target == null)
            {
                this._context.DelayRC(GameActionRCCollection.RC_GAME_ACTION_DMG, args);
                return;
            }

            if (rcArgs.dmgRecord.value > 0)
            {
                this._PlayActionHitEffect(rcArgs.actionId, target);
            }

            // 伤害跳字
            var jumpTextApi = this._context.uiApi.jumpTextApi;
            var randomStyle = GameMath.RandomRange(1, 5);//1-4
            var jumpPos = this.WorldToScreenPoint(target.transformCom.position);
            jumpPos.y += 50;
            jumpTextApi.JumpText_Dmg(jumpPos, dmgRecord.dmgType, dmgRecord.isCrit, randomStyle, GameMath.Floor(dmgRecord.value).ToString(), 0.6f);

            if (target is GameRoleEntityR targetRole)
            {
                // 骨骼抖动
                var angle = 0;
                var amplitude = 0.08f;
                var frequency = 30;
                var duration = 0.1f;
                this._context.domainApi.transformApi.Shake(targetRole.bodyCom.prefabGO.transform, angle, amplitude, frequency, duration);
                // 受击闪烁(非潜行状态)
                if (targetRole.fsmCom.stateType != GameRoleStateType.Stealth)
                {
                    var typeId = (int)GameShaderEffectType.HitFlash;
                    var renderers = targetRole.bodyCom.renderers;
                    this._context.domainApi.shaderEffectApi.PlayShaderEffect(typeId, renderers);
                }
            }
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
            var rcArgs = (GameActionRCArgs_Heal)args;
            ref var healRecord = ref rcArgs.healRecord;
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
                this._PlayActionHitEffect(rcArgs.actionId, target);
            }
        }

        private void _OnAction_LaunchProjectile(object args)
        {
            var rcArgs = (GameActionRCArgs_LaunchProjectile)args;
            ref var record = ref rcArgs.record;
            ref var actorIdArgs = ref record.actorIdArgs;
            var actor = this._context.FindEntity(actorIdArgs);
            ref var targetIdArgs = ref record.targetIdArgs;
            var target = this._context.FindEntity(targetIdArgs);
            if (actor == null || target == null)
            {
                this._context.DelayRC(GameActionRCCollection.RC_GAME_ACTION_LAUNCH_PROJECTILE, args);
                return;
            }
            this._PlayActionHitEffect(rcArgs.actionId, target);
        }

        private void _OnAction_KnockBack(object args)
        {
            var rcArgs = (GameActionRCArgs_KnockBack)args;
            ref var record = ref rcArgs.record;
            ref var actorIdArgs = ref record.actorIdArgs;
            var actor = this._context.FindEntity(actorIdArgs);
            ref var targetIdArgs = ref record.targetIdArgs;
            var target = this._context.FindEntity(targetIdArgs);
            if (actor == null || target == null)
            {
                this._context.DelayRC(GameActionRCCollection.RC_GAME_ACTION_KNOCK_BACK, args);
                return;
            }
            this._PlayActionHitEffect(rcArgs.actionId, target);
        }

        private void _OnAction_AttributeModify(object args)
        {
            var rcArgs = (GameActionRCArgs_AttributeModify)args;
            ref var record = ref rcArgs.record;
            ref var actorIdArgs = ref record.actorIdArgs;
            var actor = this._context.FindEntity(actorIdArgs);
            ref var targetIdArgs = ref record.targetIdArgs;
            var target = this._context.FindEntity(targetIdArgs);
            if (actor == null || target == null)
            {
                this._context.DelayRC(GameActionRCCollection.RC_GAME_ACTION_ATTRIBUTE_MODIFY, args);
                return;
            }
            this._PlayActionHitEffect(rcArgs.actionId, target);
        }

        private void _OnAction_AttachBuff(object args)
        {
            var rcArgs = (GameActionRCArgs_AttachBuff)args;
            ref var record = ref rcArgs.record;
            ref var actorIdArgs = ref record.actorIdArgs;
            var actor = this._context.FindEntity(actorIdArgs);
            ref var targetIdArgs = ref record.targetIdArgs;
            var target = this._context.FindEntity(targetIdArgs);
            if (actor == null || target == null)
            {
                this._context.DelayRC(GameActionRCCollection.RC_GAME_ACTION_ATTACH_BUFF, args);
                return;
            }
            this._PlayActionHitEffect(rcArgs.actionId, target);
        }

        private void _OnAction_CharacterTransform(object args)
        {
            var rcArgs = (GameActionRCArgs_CharacterTransform)args;
            ref var record = ref rcArgs.record;
            ref var actorIdArgs = ref record.actorIdArgs;
            var actor = this._context.FindEntity(actorIdArgs);
            ref var targetIdArgs = ref record.targetIdArgs;
            var target = this._context.FindEntity(targetIdArgs);
            if (actor == null || target == null)
            {
                this._context.DelayRC(GameActionRCCollection.RC_GAME_ACTION_TRANSFORM, args);
                return;
            }
            this._PlayActionHitEffect(rcArgs.actionId, target);
        }

        private void _OnAction_Stealth(object args)
        {
            var rcArgs = (GameActionRCArgs_Stealth)args;
            ref var record = ref rcArgs.record;
            ref var actorIdArgs = ref record.actorIdArgs;
            var actor = this._context.FindEntity(actorIdArgs);
            ref var targetIdArgs = ref record.targetIdArgs;
            var target = this._context.FindEntity(targetIdArgs);
            if (actor == null || target == null)
            {
                this._context.DelayRC(GameActionRCCollection.RC_GAME_ACTION_STEALTH, args);
                return;
            }
            this._PlayActionHitEffect(rcArgs.actionId, target);
        }
    }
}