using System.Collections.Generic;
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

        public Dictionary<GameProjectileStateType, GameProjectileStateModelBase> stateModelDict;
        public GameProjectileFSMCom()
        {
            idleStateModel = new GameProjectileStateModel_Idle();
            fixedDirectionStateModel = new GameProjectileStateModel_FixedDirection();
            lockOnEntityStateModel = new GameProjectileStateModel_LockOnEntity();
            lockOnPositionStateModel = new GameProjectileStateModel_LockOnPosition();
            attachStateModel = new GameProjectileStateModel_Attach();
            explodeStateModel = new GameProjectileStateModel_Explode();
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
            }
        }
    }
}

