using GamePlay.Core;
namespace GamePlay.Bussiness.Logic
{
    public struct GameEntitySelector
    {
        // 锚点类型
        // 阵营类型
        public GameCampType campType;
        // 实体类型
        public GameEntityType entityType;
        // 碰撞模型
        public GameColliderModelBase colliderModel;

    }
}