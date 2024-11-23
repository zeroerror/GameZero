using GamePlay.Bussiness.Logic;
using GamePlay.Core;
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
            var inputArgs = new GameRoleInputArgs();
            // 移动
            var moveDir = new GameVec2(0, 0);
            if (Input.GetKey(KeyCode.W)) { moveDir.y += 1; }
            if (Input.GetKey(KeyCode.S)) { moveDir.y -= 1; }
            if (Input.GetKey(KeyCode.A)) { moveDir.x -= 1; }
            if (Input.GetKey(KeyCode.D)) { moveDir.x += 1; }
            inputArgs.moveDir = moveDir.normalized;
            // 攻击
            if (Input.GetKeyDown(KeyCode.J)) inputArgs.skillId = 1;
            // ...
            if (inputArgs.HasInput())
            {
                var userRole = this._roleContext.userRole;
                var entityId = userRole.idCom.entityId;
                this._roleContext.context.logicContext.roleContext.SetPlayerInputArgs(entityId, inputArgs);
            }
        }
    }
}