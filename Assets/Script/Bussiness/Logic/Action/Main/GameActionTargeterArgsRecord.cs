using GameVec2 = UnityEngine.Vector2;
namespace GamePlay.Bussiness.Logic
{
    /**
     * 行为目标选取器参数记录
     * 如选取的目标实体Id参数、目标位置、目标方向等
     */
    public struct GameActionTargeterArgsRecord
    {
        public GameIdArgs targetIdArgs;
        public GameVec2 targetPosition;
        public GameVec2 targetDirection;

        public GameActionTargeterArgsRecord(in GameIdArgs targetIdArgs, in GameVec2 targetPosition, in GameVec2 targetDirection)
        {
            this.targetIdArgs = targetIdArgs;
            this.targetPosition = targetPosition;
            this.targetDirection = targetDirection;
        }
    }
}