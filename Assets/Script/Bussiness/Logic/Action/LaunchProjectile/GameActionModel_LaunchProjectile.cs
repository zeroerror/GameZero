using GameVec2 = UnityEngine.Vector2;
namespace GamePlay.Bussiness.Logic
{
    public class GameActionModel_LaunchProjectile : GameActionModelBase
    {
        public readonly int projectileId;
        public readonly GameVec2 launchOffset;

        public readonly GameProjectileBarrageType barrageType;
        public readonly GameProjectileBarrageModel_CustomLaunchOffset customLaunchOffsetModel;
        public readonly GameProjectileBarrageModel_Spread spreadModel;

        public GameActionModel_LaunchProjectile(
            int projectileId,
            in GameVec2 launchOffset,
            GameProjectileBarrageType barrageType,
            GameProjectileBarrageModel_CustomLaunchOffset customLaunchOffsetModel,
            GameProjectileBarrageModel_Spread spreadModel,
            GameEntitySelector selector
        ) : base(GameActionType.LaunchProjectile, projectileId, selector)
        {
            this.projectileId = projectileId;
            this.launchOffset = launchOffset;
            this.barrageType = barrageType;
            this.customLaunchOffsetModel = customLaunchOffsetModel;
            this.spreadModel = spreadModel;
        }

        public override string ToString()
        {
            return $"发射投射物行为: 投射物Id:{this.projectileId} 发射偏移:{this.launchOffset}";
        }

    }
}