using GameVec2 = UnityEngine.Vector2;
namespace GamePlay.Bussiness.Logic
{
    public static class GameRoleMoveUtil
    {
        public static bool TickMove(GameRoleEntity role, in GameVec2 moveDir, in GameVec2 moveDst, float dt)
        {
            // 根据方向移动
            if (!moveDir.Equals(GameVec2.zero))
            {
                var moveSpeed = role.attributeCom.GetValue(GameAttributeType.MoveSpeed);
                var moveVec = new GameVec2(moveDir.x, moveDir.y) * moveSpeed * dt;
                role.transformCom.position += moveVec;
                role.FaceTo(moveDir);
                return true;
            }

            // 根据目的地移动
            if (!moveDst.Equals(GameVec2.zero) && !role.transformCom.position.Equals(moveDst))
            {
                var moveSpeed = role.attributeCom.GetValue(GameAttributeType.MoveSpeed);
                var moveVec = moveDst - new GameVec2(role.transformCom.position.x, role.transformCom.position.y);
                var dstDir = moveVec.normalized;
                var moveDis = moveVec.magnitude;
                if (moveDis < moveSpeed * dt)
                {
                    role.transformCom.position = new GameVec2(moveDst.x, moveDst.y);
                }
                else
                {
                    role.transformCom.position += dstDir * moveSpeed * dt;
                    role.FaceTo(dstDir);
                }
                return true;
            }

            return false;
        }
    }
}