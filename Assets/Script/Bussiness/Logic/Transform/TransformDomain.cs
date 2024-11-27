
using GameVec2 = UnityEngine.Vector2;
namespace GamePlay.Bussiness.Logic
{
    public class GameTransformDomain : GameTransformDomainApi
    {
        GameContext _context;

        public GameTransformDomain()
        {
        }

        public void Inject(GameContext context)
        {
            this._context = context;
        }

        public void Dispose()
        {
        }

        public void Tick(float dt)
        {
            var roleContext = this._context.roleContext;
            roleContext.entityRepo.ForeachEntities((entity) =>
            {
                if (entity.transformCom.CheckDirty())
                {
                    this._context.rcEventService.Submit(GameTransformRCCollection.RC_GAME_TRANSFORMN_SYNC, new GameTransformRCArgs_Sync()
                    {
                        idArgs = entity.idCom.ToArgs(),
                        transComArgs = entity.transformCom.ToArgs()
                    });
                }
            });
        }
    }
}
