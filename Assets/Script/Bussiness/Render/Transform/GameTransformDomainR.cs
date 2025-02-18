using System.Collections.Generic;
using GamePlay.Bussiness.Logic;
using UnityEngine;

namespace GamePlay.Bussiness.Render
{
    public class GameTransformDomainR : GameTransformDomainApiR
    {
        GameContextR _context;

        private List<GameTransformShake> _shakeList;
        private List<GameTransformShake> _shakePool;

        public GameTransformDomainR()
        {
            this._shakeList = new List<GameTransformShake>();
            this._shakePool = new List<GameTransformShake>();
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
            this._context.BindRC(GameTransformRCCollection.RC_GAME_TRANSFORMN_SYNC, this._OnTransformSync);
            this._context.BindRC(GameTransformRCCollection.RC_GAME_TRANSFORMN_SYNC_IMMEDIATE, this._OnTransformSyncImmediate);
        }
        public void UnbindEvents()
        {
            this._context.UnbindRC(GameTransformRCCollection.RC_GAME_TRANSFORMN_SYNC, this._OnTransformSync);
            this._context.UnbindRC(GameTransformRCCollection.RC_GAME_TRANSFORMN_SYNC_IMMEDIATE, this._OnTransformSyncImmediate);
        }

        private void _OnTransformSync(object args)
        {
            var rcArgs = (GameTransformRCArgs_Sync)args;
            ref var idArgs = ref rcArgs.idArgs;
            GameEntityBase entity = this._context.FindEntity(idArgs);
            if (entity == null)
            {
                this._context.DelayRC(GameTransformRCCollection.RC_GAME_TRANSFORMN_SYNC, args);
                return;
            }
            entity.transformCom.SetByArgs(rcArgs.transArgs);
        }

        private void _OnTransformSyncImmediate(object args)
        {
            var rcArgs = (GameTransformRCArgs_SyncImmediate)args;
            ref var idArgs = ref rcArgs.idArgs;
            GameEntityBase entity = this._context.FindEntity(idArgs);
            if (entity == null)
            {
                this._context.DelayRC(GameTransformRCCollection.RC_GAME_TRANSFORMN_SYNC_IMMEDIATE, args);
                return;
            }
            entity.transformCom.SetByArgs(rcArgs.transArgs);
            entity.SetPosition(entity.transformCom.position);
        }

        public void Tick(float dt)
        {
            this._TickShake(dt);
        }

        private void _TickShake(float dt)
        {
            for (int i = this._shakeList.Count - 1; i >= 0; i--)
            {
                GameTransformShake shake = this._shakeList[i];
                shake.Tick(dt);
                if (!shake.isShaking)
                {
                    this._shakeList.RemoveAt(i);
                    this._shakePool.Add(shake);
                }
            }
        }

        public void Shake(Transform transform, float angle, float amplitude, float frequency, float duration)
        {
            GameTransformShake shake = this._GetShake();
            shake.SetShake(transform, angle, amplitude, frequency, duration);
            this._shakeList.Add(shake);
        }

        private GameTransformShake _GetShake()
        {
            if (this._shakePool.Count > 0)
            {
                GameTransformShake shake = this._shakePool[0];
                this._shakePool.RemoveAt(0);
                return shake;
            }
            return new GameTransformShake();
        }
    }
}
