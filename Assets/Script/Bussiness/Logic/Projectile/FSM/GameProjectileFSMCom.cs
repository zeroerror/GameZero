using System.Collections.Generic;
using GameVec2 = UnityEngine.Vector2;
namespace GamePlay.Bussiness.Logic
{
    public class GameProjectileFSMCom
    {
        public GameProjectileStateType stateType { get; private set; }
        public GameProjectileStateType lastStateType { get; private set; }

        public GameProjectileStateModel_Idle idleStateModel { get; private set; }
        public GameProjectileStateModel_FixedDirection fixedDirectionStateModel { get; private set; }
        public GameProjectileStateModel_LockOnEntity lockOnEntityStateModel { get; private set; }
        public GameProjectileStateModel_LockOnPosition lockOnPositionStateModel { get; private set; }
        public GameProjectileStateModel_Attach attachStateModel { get; private set; }
        public GameProjectileStateModel_Explode explodeStateModel { get; private set; }
        public GameProjectileStateModel_Destroyed destroyedStateModel { get; private set; }
        public Dictionary<GameProjectileStateType, GameProjectileStateTriggerSetEntity> triggerSetEntityDict { get; private set; }

        public GameProjectileStateType defaultStateType;

        public GameProjectileFSMCom()
        {
            idleStateModel = new GameProjectileStateModel_Idle();
            fixedDirectionStateModel = new GameProjectileStateModel_FixedDirection();
            lockOnEntityStateModel = new GameProjectileStateModel_LockOnEntity();
            lockOnPositionStateModel = new GameProjectileStateModel_LockOnPosition();
            attachStateModel = new GameProjectileStateModel_Attach();
            explodeStateModel = new GameProjectileStateModel_Explode();
            destroyedStateModel = new GameProjectileStateModel_Destroyed();
            triggerSetEntityDict = new Dictionary<GameProjectileStateType, GameProjectileStateTriggerSetEntity>();
            defaultStateType = GameProjectileStateType.None;
        }

        public void Clear()
        {
            idleStateModel.Clear();
            fixedDirectionStateModel.Clear();
            lockOnEntityStateModel.Clear();
            lockOnPositionStateModel.Clear();
            attachStateModel.Clear();
            explodeStateModel.Clear();
            destroyedStateModel.Clear();
            triggerSetEntityDict.Clear();
            stateType = GameProjectileStateType.None;
            lastStateType = GameProjectileStateType.None;
            foreach (var kv in triggerSetEntityDict)
            {
                kv.Value.Clear();
            }
        }

        public void EnterIdle()
        {
            this.SwitchToState(GameProjectileStateType.Idle);
        }

        public void EnterFixedDirection()
        {
            this.SwitchToState(GameProjectileStateType.FixedDirection);
        }

        public void EnterLockOnEntity(GameEntityBase targetEntity)
        {
            this.SwitchToState(GameProjectileStateType.LockOnEntity);
            lockOnEntityStateModel.SetLockOnEntity(targetEntity);
        }

        public void EnterLockOnPosition(in GameVec2 lockOnPosition)
        {
            this.SwitchToState(GameProjectileStateType.LockOnPosition);
            lockOnPositionStateModel.SetLockOnPosition(lockOnPosition);
        }

        public void EnterAttach()
        {
            this.SwitchToState(GameProjectileStateType.Attach);
        }

        public void EnterExplode()
        {
            this.SwitchToState(GameProjectileStateType.Explode);
        }

        public void EnterDestroyed()
        {
            this.SwitchToState(GameProjectileStateType.Destroyed);
        }

        public void SwitchToState(GameProjectileStateType nextState)
        {
            this.lastStateType = this.stateType;
            this.stateType = nextState;
            switch (nextState)
            {
                case GameProjectileStateType.Idle:
                    idleStateModel.Clear();
                    break;
                case GameProjectileStateType.FixedDirection:
                    fixedDirectionStateModel.Clear();
                    break;
                case GameProjectileStateType.LockOnEntity:
                    lockOnEntityStateModel.Clear();
                    break;
                case GameProjectileStateType.LockOnPosition:
                    lockOnPositionStateModel.Clear();
                    break;
                case GameProjectileStateType.Attach:
                    attachStateModel.Clear();
                    break;
                case GameProjectileStateType.Explode:
                    explodeStateModel.Clear();
                    break;
                case GameProjectileStateType.Destroyed:
                    destroyedStateModel.Clear();
                    break;
            }
        }
    }
}

