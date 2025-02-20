using System;
using GamePlay.Bussiness.Logic;
using GamePlay.Core;
using GamePlay.Infrastructure;
using UnityEngine;
using GameVec2 = UnityEngine.Vector2;

namespace GamePlay.Bussiness.Render
{
    public class GameDirectorStateDomain_FightPreparingR : GameDirectorStateDomainBaseR
    {
        public GameDirectorStateDomain_FightPreparingR(GameDirectorDomainR directorDomain) : base(directorDomain)
        {
        }

        public override void BindEvents()
        {
            this._context.BindRC(GameDirectorRCCollection.RC_GAME_DIRECTOR_STATE_ENTER_FIGHT_PREPARING, this._OnStateEnter);
        }

        public override void UnbindEvents()
        {
            this._context.UnbindRC(GameDirectorRCCollection.RC_GAME_DIRECTOR_STATE_ENTER_FIGHT_PREPARING, this._OnStateEnter);
        }

        private void _OnStateEnter(object args)
        {
            this.Enter(this._context.director, args);
        }

        public override void Enter(GameDirectorEntityR director, object args = null)
        {
            // 体验音效测试代码
            string[] urls = new string[] { "SFX/audio_clip_gun_shot", "SFX/audio_clip_gun_reload", "SFX/audio_clip_bow_shot", "SFX/audio_clip_bow_hit" };
            urls?.Foreach((url) =>
            {
                this._context.cmdBufferService.AddIntervalCmd(0.5f, () =>
                {
                    var randomDelay = GameMathF.RandomRange(0.5f, 2.5f);
                    this._context.cmdBufferService.AddDelayCmd(randomDelay, () =>
                    {
                        var sfx = this._context.audioService.PlaySFX(url, 0);
                        sfx.volume = 0.1f;
                    });
                });
            });

            director.fsmCom.EnterFightPreparing();
            GameLogger.DebugLog("R 导演 - 进入战斗准备状态");
        }

        protected override void _Tick(GameDirectorEntityR director, float frameTime)
        {
        }

        public override void ExitTo(GameDirectorEntityR director, GameDirectorStateType toState)
        {
            base.ExitTo(director, toState);
        }

    }
}