using GameVec2 = UnityEngine.Vector2;
namespace GamePlay.Bussiness.Logic
{
    public static class GameRoleMoveUtil
    {
        public static void TickMove(GameRoleEntity role, float dt, out GameVec2 moveDir)
        {
            moveDir = GameVec2.zero;
            var inputCom = role.inputCom;
            if (!inputCom.TryGetInputArgs(out var inputArgs)) return;

            // 根据方向移动
            moveDir = inputArgs.moveDir;
            if (moveDir != GameVec2.zero)
            {
                var moveSpeed = role.attributeCom.GetValue(GameAttributeType.MoveSpeed);
                var moveVec = new GameVec2(moveDir.x, moveDir.y) * moveSpeed * dt;
                role.transformCom.position += moveVec;
                role.FaceTo(moveDir);
                return;
            }

            // 根据目的地移动
            var moveDst = inputArgs.moveDst;
            if (moveDst != GameVec2.zero && role.transformCom.position != moveDst)
            {
                var moveSpeed = role.attributeCom.GetValue(GameAttributeType.MoveSpeed);
                var moveVec = moveDst - new GameVec2(role.transformCom.position.x, role.transformCom.position.y);
                moveDir = moveVec.normalized;
                var moveDis = moveVec.magnitude;
                if (moveDis < moveSpeed * dt)
                {
                    role.transformCom.position = new GameVec2(moveDst.x, moveDst.y);
                    return;
                }
                role.transformCom.position += moveDir * moveSpeed * dt;
                role.FaceTo(moveDir);
            }
        }
    }
}