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
            // 移动
            var moveDir = new GameVec2(0, 0);
            var hasMoveDir = false;
            if (Input.GetKey(KeyCode.W)) { moveDir.y += 1; hasMoveDir = true; }
            if (Input.GetKey(KeyCode.S)) { moveDir.y -= 1; hasMoveDir = true; }
            if (Input.GetKey(KeyCode.A)) { moveDir.x -= 1; hasMoveDir = true; }
            if (Input.GetKey(KeyCode.D)) { moveDir.x += 1; hasMoveDir = true; }
            // ...
            if (hasMoveDir)
            {
                var userRole = this._roleContext.userRole;
                var entityId = userRole.idCom.entityId;
                moveDir.Normalize();
                var inputArgs = new GameRoleInputArgs()
                {
                    moveDir = moveDir,
                    faceDir = moveDir,
                };
                this._roleContext.context.logicContext.roleContext.SetPlayerInputArgs(entityId, inputArgs);
            }
        }
    }
}