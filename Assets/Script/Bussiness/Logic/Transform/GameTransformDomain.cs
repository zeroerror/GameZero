using GamePlay.Core;
using GamePlay.Infrastructure;
using GameVec2 = UnityEngine.Vector2;
namespace GamePlay.Bussiness.Logic
{
    public class GameTransformDomain : GameTransformDomainApi
    {
        GameContext _context;
        GameTransformContext _transformContext => this._context.transformContext;

        public GameTransformDomain()
        {
        }

        public void Inject(GameContext context)
        {
            this._context = context;
        }

        public void Destroy()
        {
        }

        public void Tick(float dt)
        {
            this._context.roleContext.repo.ForeachEntities(this._RCTransformDirty);
            this._context.projectileContext.repo.ForeachEntities(this._RCTransformDirty);

            this._transformContext.posActions.Foreach((posAction) =>
            {
                if (!posAction.transCom.isEnable || posAction.Tick(dt))
                {
                    this._context.cmdBufferService.AddDelayCmd(0, () =>
                    {
                        this._transformContext.posActions.Remove(posAction);
                    });
                }
            });
        }

        private void _RCTransformDirty(GameEntityBase entity)
        {
            if (!entity.transformCom.CheckDirty()) return;
            this._context.SubmitRC(GameTransformRCCollection.RC_GAME_TRANSFORMN_SYNC, new GameTransformRCArgs_Sync()
            {
                idArgs = entity.idCom.ToArgs(),
                transArgs = entity.transformCom.ToArgs()
            });
        }

        public void ToPosition(GameTransformCom transCom, in GameVec2 toPos, float duration, GameEasingType easingType)
        {
            var action = new GameTransformPosAction(
                transCom,
                toPos,
                duration,
                easingType
            );
            this._transformContext.posActions.Add(action);
        }
    }
}
