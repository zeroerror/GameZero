using Unity.VisualScripting;

namespace GamePlay.Bussiness.Logic
{
    public class GameRoleDomain
    {
        GameContext _context;
        GameRoleContext _roleContext => this._context.roleContext;

        public GameRoleDomain_FSM fsmDomain { get; private set; }

        public GameRoleDomain(GameContext context)
        {
            this._context = context;
            this.fsmDomain = new GameRoleDomain_FSM(context);
            this.Create(1000);//test
        }

        public void Dispose()
        {
            this.fsmDomain.Dispose();
        }

        public void Collect()
        {
        }

        public GameRoleEntity Create(int typeId)
        {
            var e = this._roleContext.factory.Load(typeId);
            this._roleContext.repo.TryAdd(e);
            this.fsmDomain.EnterState(GameRoleStateType.Idle, e);
            // 提交RC事件
            return e;
        }

        public void Tick(float dt)
        {
            var repo = this._roleContext.repo;
            repo.ForeachEntities((entity) =>
            {
                this.fsmDomain.Tick(dt, entity);
                entity.Tick(dt);
            });
        }
    }
}
