using GamePlay.Bussiness.Logic;
using GamePlay.Core;
namespace GamePlay.Bussiness.Renderer
{
    public abstract class GameRoleStateDomainR : GameStateBase<GameRoleEntityR>
    {
        protected GameContextR _context;
        protected GameContext _logicContext => _context.logicContext;
        protected GameEventService _rcEventService => _logicContext.rcEventService;

        public GameRoleStateDomainR(GameContextR context)
        {
            this._context = context;
        }

        public override abstract void Tick(float dt, GameRoleEntityR role);
        public override abstract void Enter(GameRoleEntityR role);
        public override void Exit(GameStateBase<GameRoleEntityR> nextState, GameRoleEntityR obj)
        {
            Exit(nextState as GameRoleStateDomainR, obj);
        }
        public abstract void Exit(GameRoleStateDomainR nextState, GameRoleEntityR obj);
    }

}