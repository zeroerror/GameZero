namespace GamePlay.Bussiness.Logic
{
    public class GameRoleAIDomain_Idle
    {
        GameContext _context;

        public GameRoleAIDomain_Idle()
        {
        }

        public void Inject(GameContext context)
        {
            this._context = context;
        }

        public void Destroy()
        {
        }

        public void Tick(GameRoleEntity role, float dt)
        {
            var aiCom = role.aiCom;
            var idleState = aiCom.idleState;
            idleState.stateTime += dt;
            if (idleState.stateTime > 1)
            {
                this._context.domainApi.roleApi.apApi.TryEnter(role, GameRoleAIStateType.Attack);
            }
        }
    }
}