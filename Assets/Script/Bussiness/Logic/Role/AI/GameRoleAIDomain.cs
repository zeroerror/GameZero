using GamePlay.Core;
using GamePlay.Infrastructure;

namespace GamePlay.Bussiness.Logic
{
    public class GameRoleAIDomain : GameRoleAIDomainApi
    {
        GameContext _context;
        GameRoleContext _roleContext => this._context.roleContext;

        GameRoleAIDomain_Idle _idleDomain;
        GameRoleAIDomain_Attack _attackDomain;
        GameRoleAIDomain_Follow _followDomain;

        public GameRoleAIDomain()
        {
            this._idleDomain = new GameRoleAIDomain_Idle();
            this._attackDomain = new GameRoleAIDomain_Attack();
            this._followDomain = new GameRoleAIDomain_Follow();
        }

        public void Inject(GameContext context)
        {
            this._context = context;
            this._idleDomain.Inject(context);
            this._attackDomain.Inject(context);
            this._followDomain.Inject(context);
        }

        public void Destroy()
        {
            this._idleDomain.Destroy();
            this._attackDomain.Destroy();
            this._followDomain.Destroy();
        }

        public void Tick(float dt)
        {
            var repo = this._roleContext.repo;
            repo.ForeachEntities((entity) =>
            {
                var aiCom = entity.aiCom;
                var aiStateType = aiCom.aiStateType;
                switch (aiStateType)
                {
                    case GameRoleAIStateType.Idle:
                        this._idleDomain.Tick(entity, dt);
                        break;
                    case GameRoleAIStateType.Attack:
                        this._attackDomain.Tick(entity, dt);
                        break;
                    case GameRoleAIStateType.Follow:
                        this._followDomain.Tick(entity, dt);
                        break;
                    default:
                        GameLogger.LogError("GameRoleAIDomain::Tick: 未处理的状态类型: " + aiStateType);
                        break;
                }
            });
        }

        public void TryEnter(GameRoleEntity role, GameRoleAIStateType stateType)
        {
            var aiCom = role.aiCom;
            switch (stateType)
            {
                case GameRoleAIStateType.Idle:
                    aiCom.EnterIdle();
                    break;
                case GameRoleAIStateType.Attack:
                    aiCom.EnterAttack();
                    break;
                case GameRoleAIStateType.Follow:
                    aiCom.EnterFollow();
                    break;
                default:
                    GameLogger.LogError("GameRoleAIDomain::TryEnter: 未处理的状态类型: " + stateType);
                    break;
            }
        }

    }
}