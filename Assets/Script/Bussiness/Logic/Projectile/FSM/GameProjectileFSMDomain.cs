using System.Collections.Generic;
namespace GamePlay.Bussiness.Logic
{
    public class GameProjectileFSMDomain : GameProjectileFSMDomainApi
    {
        GameContext _context;
        GameProjectileContext _projectileContext => _context.projectileContext;

        private GameProjectileStateDomainBase _anyStateDomain;
        private Dictionary<GameProjectileStateType, GameProjectileStateDomainBase> _stateDomainDict;

        private GameProjectileTriggerDomain _triggerDomain;

        public GameProjectileFSMDomain()
        {
            this._anyStateDomain = new GameProjectileStateDomain_Any();

            this._stateDomainDict = new Dictionary<GameProjectileStateType, GameProjectileStateDomainBase>
            {
                { GameProjectileStateType.Idle, new GameProjectileStateDomain_Idle() },
                { GameProjectileStateType.FixedDirection, new GameProjectileStateDomain_FixedDirection() },
                { GameProjectileStateType.LockOnEntity, new GameProjectileStateDomain_LockOnEntity() },
                { GameProjectileStateType.LockOnPosition, new GameProjectileStateDomain_LockOnPosition() },
                { GameProjectileStateType.Attach, new GameProjectileStateDomain_Attach() },
                { GameProjectileStateType.Explode, new GameProjectileStateDomain_Explode() },
                { GameProjectileStateType.Destroyed, new GameProjectileStateDomain_Destroyed() }
            };

            this._triggerDomain = new GameProjectileTriggerDomain();
        }

        public void Inject(GameContext context)
        {
            this._context = context;
            foreach (var stateDomain in this._stateDomainDict.Values)
            {
                stateDomain.Inject(context);
            }
            this._anyStateDomain.Inject(context);
            this._triggerDomain.Inject(context);
        }

        public void Destroy()
        {
            this._triggerDomain.Destroy();
        }

        public void Tick(GameProjectileEntity projectile, float dt)
        {
            var fsmCom = projectile.fsmCom;
            var stateType = fsmCom.stateType;
            if (stateType == GameProjectileStateType.None) return;
            // 状态逻辑
            this._anyStateDomain.Tick(projectile, dt);
            if (!this._stateDomainDict.TryGetValue(stateType, out var stateDomain)) return;
            stateDomain.Tick(projectile, dt);
            // 触发器逻辑
            var nextStateType = this._triggerDomain.Tick(projectile, dt);
            if (nextStateType != GameProjectileStateType.None) this._Enter(projectile, nextStateType);
        }

        public void InitFSM(GameProjectileEntity projectile)
        {
            this._triggerDomain.InitFSMTrigger(projectile);
            var fsmCom = projectile.fsmCom;
            var model = projectile.model;
            foreach (var stateModel in model.stateModelDict.Values)
            {
                switch (stateModel)
                {
                    case GameProjectileStateModel_FixedDirection fixedDirectionModel:
                        fsmCom.fixedDirectionState.SetModel(fixedDirectionModel);
                        break;
                    case GameProjectileStateModel_LockOnEntity lockOnEntityModel:
                        fsmCom.lockOnEntityState.SetModel(lockOnEntityModel);
                        break;
                    case GameProjectileStateModel_LockOnPosition lockOnPositionModel:
                        fsmCom.lockOnPositionState.SetModel(lockOnPositionModel);
                        break;
                    case GameProjectileStateModel_Attach attachModel:
                        fsmCom.attachState.SetModel(attachModel);
                        break;
                }
                this._Enter(projectile, fsmCom.defaultStateType);
            }
        }

        public bool TryEnter(GameProjectileEntity entity, GameProjectileStateType state)
        {
            if (!this._stateDomainDict.TryGetValue(state, out var stateDomain)) return false;
            var check = stateDomain.CheckEnter(entity);
            if (!check) return false;
            this._Enter(entity, state);
            return true;
        }

        private void _Enter(GameProjectileEntity entity, GameProjectileStateType state)
        {
            this._ExitToState(entity, state);
            if (!this._stateDomainDict.TryGetValue(state, out var stateDomain)) return;
            stateDomain.Enter(entity);
            if (entity.fsmCom.triggerSetEntityDict.TryGetValue(state, out var triggerSetEntity)) triggerSetEntity.Clear();
            entity.physicsCom.ClearCollided();
        }

        private void _ExitToState(GameProjectileEntity entity, GameProjectileStateType toState)
        {
            var fsmCom = entity.fsmCom;
            var stateType = fsmCom.stateType;
            if (!this._stateDomainDict.TryGetValue(stateType, out var stateDomain)) return;
            stateDomain.ExitTo(entity, toState);
        }
    }
}