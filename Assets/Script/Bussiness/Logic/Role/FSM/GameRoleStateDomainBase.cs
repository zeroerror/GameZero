using GamePlay.Core;
namespace GamePlay.Bussiness.Logic
{
    public abstract class GameRoleStateDomainBase : GameStateBase<GameRoleEntity>
    {
        protected GameContext _context;

        public GameRoleStateDomainBase(GameContext context)
        {
            this._context = context;
        }

        public override abstract void Tick(float dt, GameRoleEntity role);
        public override abstract void Enter(GameRoleEntity role);
        public override void Exit(GameStateBase<GameRoleEntity> nextState, GameRoleEntity obj)
        {
            Exit(nextState as GameRoleStateDomainBase, obj);
        }
        public abstract void Exit(GameRoleStateDomainBase nextState, GameRoleEntity obj);
    }

}