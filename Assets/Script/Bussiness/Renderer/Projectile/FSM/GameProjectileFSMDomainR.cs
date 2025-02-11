using System.Collections.Generic;
using GamePlay.Bussiness.Logic;

namespace GamePlay.Bussiness.Render
{
    public class GameProjectileFSMDomain : GameProjectileFSMDomainApiR
    {
        GameContextR _context;
        GameProjectileContextR _projectileContext => _context.projectileContext;
        private Dictionary<GameProjectileStateType, GameProjectileStateDomainBaseR> _stateDomainDict = new Dictionary<GameProjectileStateType, GameProjectileStateDomainBaseR>();

        public GameProjectileFSMDomain()
        {
            this._stateDomainDict.Add(GameProjectileStateType.Idle, new GameProjectileStateDomain_IdleR());
            this._stateDomainDict.Add(GameProjectileStateType.FixedDirection, new GameProjectileStateDomain_FixedDirectionR());
            this._stateDomainDict.Add(GameProjectileStateType.LockOnEntity, new GameProjectileStateDomain_LockOnEntityR());
            this._stateDomainDict.Add(GameProjectileStateType.LockOnPosition, new GameProjectileStateDomain_LockOnPositionR());
            this._stateDomainDict.Add(GameProjectileStateType.Attach, new GameProjectileStateDomain_AttachR());
            this._stateDomainDict.Add(GameProjectileStateType.Explode, new GameProjectileStateDomain_ExplodeR());
            this._stateDomainDict.Add(GameProjectileStateType.Destroyed, new GameProjectileStateDomain_DestroyedR());
        }

        public void Inject(GameContextR context)
        {
            this._context = context;
            foreach (var stateDomain in this._stateDomainDict.Values)
            {
                stateDomain.Inject(context);
            }
            this._BindEvents();
        }

        public void Destroy()
        {
            this._UnbindEvents();
        }

        private void _BindEvents()
        {
            foreach (var stateDomain in this._stateDomainDict.Values) stateDomain.BindEvents();
        }

        private void _UnbindEvents()
        {
            foreach (var stateDomain in this._stateDomainDict.Values) stateDomain.UnbindEvents();
        }

        public void Enter(GameProjectileEntityR entity, GameProjectileStateType state)
        {
        }

        public void Tick(GameProjectileEntityR entity, float dt)
        {
            var fsmCom = entity.fsmCom;
            var stateType = fsmCom.stateType;
            if (stateType == GameProjectileStateType.None) return;
            // 状态逻辑
            if (!this._stateDomainDict.TryGetValue(stateType, out var stateDomain)) return;
            stateDomain.Tick(entity, dt);
        }

    }
}