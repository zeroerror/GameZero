using GamePlay.Bussiness.Logic;
using GamePlay.Core;
using UnityEngine;
using GameVec2 = UnityEngine.Vector2;
namespace GamePlay.Bussiness.Renderer
{
    public class GameRoleInputDomainR
    {
        GameContextR _context;
        private GameRoleContextR _roleContext => this._context.roleContext;

        public GameRoleInputDomainR()
        {
        }

        public void Inject(GameContextR context)
        {
            this._context = context;
        }

        public void Destroy()
        {
        }

        public void Tick()
        {
            return;// 自走棋模式
            var inputArgs = new GameRoleInputArgs();
            var userRole = this._roleContext.userRole;
            if (userRole != null)
            {
                // 移动
                var moveDir = new GameVec2(0, 0);
                if (Input.GetKey(KeyCode.W)) { moveDir.y += 1; }
                if (Input.GetKey(KeyCode.S)) { moveDir.y -= 1; }
                if (Input.GetKey(KeyCode.A)) { moveDir.x -= 1; }
                if (Input.GetKey(KeyCode.D)) { moveDir.x += 1; }
                inputArgs.moveDir = moveDir.normalized;
                // 攻击
                if (Input.GetKeyDown(KeyCode.J))
                {
                    if (userRole.skillCom.TryGetByIndex(0, out var skill))
                    {
                        inputArgs.skillId = skill.skillModel.typeId;
                    }
                }
                if (Input.GetKeyDown(KeyCode.K))
                {
                    if (userRole.skillCom.TryGetByIndex(1, out var skill))
                    {
                        inputArgs.skillId = skill.skillModel.typeId;
                    }
                }
                if (Input.GetKeyDown(KeyCode.L))
                {
                    if (userRole.skillCom.TryGetByIndex(2, out var skill))
                    {
                        inputArgs.skillId = skill.skillModel.typeId;
                    }
                }
                // ...
                if (inputArgs.HasInput())
                {
                    var entityId = userRole.idCom.entityId;
                    this._context.logicContext.roleContext.SetPlayerInputArgs(entityId, inputArgs);
                }
            }
        }
    }
}