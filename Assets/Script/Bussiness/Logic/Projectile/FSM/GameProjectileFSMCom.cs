using System.Collections.Generic;
using GameVec2 = UnityEngine.Vector2;
namespace GamePlay.Bussiness.Logic
{
    public class GameProjectileFSMCom
    {
        public GameProjectileStateType stateType { get; private set; }
        public GameProjectileStateType lastStateType { get; private set; }
        public bool isInvalid => stateType == GameProjectileStateType.None || stateType == GameProjectileStateType.Destroyed;

        public GameProjectileState_Any anyState { get; private set; }
        public GameProjectileState_Idle idleState { get; private set; }
        public GameProjectileState_FixedDirection fixedDirectionState { get; private set; }
        public GameProjectileState_LockOnEntity lockOnEntityState { get; private set; }
        public GameProjectileState_LockOnPosition lockOnPositionState { get; private set; }
        public GameProjectileState_Attach attachState { get; private set; }
        public GameProjectileState_Explode explodeState { get; private set; }
        public GameProjectileState_Destroyed destroyedState { get; private set; }
        public Dictionary<GameProjectileStateType, GameProjectileTriggerSetEntity> triggerSetEntityDict { get; private set; }

        public GameProjectileStateType defaultStateType;

        public GameProjectileFSMCom()
        {
            anyState = new GameProjectileState_Any();
            idleState = new GameProjectileState_Idle();
            fixedDirectionState = new GameProjectileState_FixedDirection();
            lockOnEntityState = new GameProjectileState_LockOnEntity();
            lockOnPositionState = new GameProjectileState_LockOnPosition();
            attachState = new GameProjectileState_Attach();
            explodeState = new GameProjectileState_Explode();
            destroyedState = new GameProjectileState_Destroyed();
            triggerSetEntityDict = new Dictionary<GameProjectileStateType, GameProjectileTriggerSetEntity>();
            defaultStateType = GameProjectileStateType.None;
        }

        public void Clear()
        {
            anyState.Clear();
            idleState.Clear();
            fixedDirectionState.Clear();
            lockOnEntityState.Clear();
            lockOnPositionState.Clear();
            attachState.Clear();
            explodeState.Clear();
            destroyedState.Clear();
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

        public void EnterFixedDirection(in GameVec2 direction)
        {
            this.SwitchToState(GameProjectileStateType.FixedDirection);
            fixedDirectionState.direction = direction;
        }

        public void EnterLockOnEntity()
        {
            this.SwitchToState(GameProjectileStateType.LockOnEntity);
        }

        public void EnterLockOnPosition()
        {
            this.SwitchToState(GameProjectileStateType.LockOnPosition);
        }

        public void EnterAttach(in GameActionTargeterArgs targeter)
        {
            this.SwitchToState(GameProjectileStateType.Attach);
            attachState.SetTargeter(targeter);
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
                    idleState.Clear();
                    break;
                case GameProjectileStateType.FixedDirection:
                    fixedDirectionState.Clear();
                    break;
                case GameProjectileStateType.LockOnEntity:
                    lockOnEntityState.Clear();
                    break;
                case GameProjectileStateType.LockOnPosition:
                    lockOnPositionState.Clear();
                    break;
                case GameProjectileStateType.Attach:
                    attachState.Clear();
                    break;
                case GameProjectileStateType.Explode:
                    explodeState.Clear();
                    break;
                case GameProjectileStateType.Destroyed:
                    destroyedState.Clear();
                    break;
            }
        }
    }
}

