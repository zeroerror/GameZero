using GameVec2 = UnityEngine.Vector2;
using GamePlay.Core;
namespace GamePlay.Bussiness.Logic
{

    /**
     * 行为目标选取器接口
     * 如选取的目标实体、目标位置、目标方向等
     */
    public interface IGameActionTargeter
    {
        public GameRoleEntity targetRole { get; set; }
        public GameVec2 targetPos { get; set; }
        public GameVec2 targetDirection { get; set; }
    }

    public static class IGameActionTargeterExtension
    {
        public static void SetTarget(IGameActionTargeter targeter, GameEntity target)
        {
            if (target is GameRoleEntity)
            {
                targeter.targetRole = (GameRoleEntity)target;
            }
            else
            {
                GameLogger.Error("SetTarget target is not GameRoleEntity");
            }
        }
    }
}