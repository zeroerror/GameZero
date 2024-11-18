using GamePlay.Bussiness.Logic;
using UnityEngine;
using GameVec2 = UnityEngine.Vector2;
namespace GamePlay.Bussiness.Renderer
{
    public class GameRoleInputDomainR
    {
        GameContextR _context;
        GameRoleContextR _roleContext => this._context.roleContext;

        public GameRoleInputDomainR()
        {
        }

        public void Inject(GameContextR context)
        {
            this._context = context;
        }

        public void Dispose()
        {
        }

        public void Tick()
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                var userRole = this._roleContext.userRole;
                var entityId = userRole.idCom.entityId;
                var inputArgs = new GameRoleInputArgs()
                {
                    moveDir = new GameVec2(0, 1),
                    faceDir = new GameVec2(0, 1),
                };
                this._roleContext.context.logicContext.roleContext.SetPlayerInputArgs(entityId, inputArgs);
            }
        }
    }
}