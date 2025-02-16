using System.Collections.Generic;
using GamePlay.Bussiness.Logic;
namespace GamePlay.Bussiness.Render
{
    public class GameProjectileFSMCom
    {
        public GameProjectileStateType stateType { get; private set; }
        public GameProjectileStateType lastStateType { get; private set; }

        public GameProjectileState_Idle idleState { get; private set; }
        public GameProjectileState_FixedDirection fixedDirectionState { get; private set; }
        public GameProjectileState_LockOnEntity lockOnEntityState { get; private set; }
        public GameProjectileState_LockOnPosition lockOnPositionState { get; private set; }
        public GameProjectileState_Attach attachState { get; private set; }
        public GameProjectileState_Explode explodeState { get; private set; }

        public Dictionary<GameProjectileStateType, GameProjectileStateBase> stateModelDict;
        public GameProjectileFSMCom()
        {
            idleState = new GameProjectileState_Idle();
            fixedDirectionState = new GameProjectileState_FixedDirection();
            lockOnEntityState = new GameProjectileState_LockOnEntity();
            lockOnPositionState = new GameProjectileState_LockOnPosition();
            attachState = new GameProjectileState_Attach();
            explodeState = new GameProjectileState_Explode();
        }

        public void EnterIdle()
        {
            this.SwitchToState(GameProjectileStateType.Idle);
        }

        public void EnterFixedDirection()
        {
            this.SwitchToState(GameProjectileStateType.FixedDirection);
        }

        public void EnterLockOnEntity()
        {
            this.SwitchToState(GameProjectileStateType.LockOnEntity);
        }

        public void EnterLockOnPosition()
        {
            this.SwitchToState(GameProjectileStateType.LockOnPosition);
        }

        public void EnterAttach()
        {
            this.SwitchToState(GameProjectileStateType.Attach);
        }

        public void EnterExplode()
        {
            this.SwitchToState(GameProjectileStateType.Explode);
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
            }
        }
    }
}

