using GameVec2 = UnityEngine.Vector2;
namespace GamePlay.Bussiness.Logic
{
    /**
     * 行为目标选取器参数
     * 如选取的目标实体、目标位置、目标方向等
     */
    public struct GameActionTargeterArgs
    {
        public GameEntityBase targetEntity;
        public GameVec2 targetPosition;
        public GameVec2 targetDirection;

        public GameActionTargeterArgs(GameEntityBase targetEntity, in GameVec2 targetPosition, in GameVec2 targetDirection)
        {
            this.targetEntity = targetEntity;
            this.targetPosition = targetPosition;
            this.targetDirection = targetDirection;
        }
    }
}