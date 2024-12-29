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
            GameEntitySelector selector,
            GameActionPreconditionSetModel preconditionSet,
            in GameVec2 launchOffset,
            GameProjectileBarrageType barrageType,
            GameProjectileBarrageModel_CustomLaunchOffset customLaunchOffsetModel,
            GameProjectileBarrageModel_Spread spreadModel
        ) : base(GameActionType.LaunchProjectile, projectileId, selector, preconditionSet)
        {
            this.projectileId = projectileId;
            this.launchOffset = launchOffset;
            this.barrageType = barrageType;
            this.customLaunchOffsetModel = customLaunchOffsetModel;
            this.spreadModel = spreadModel;
        }

        public override GameActionModelBase GetCustomModel(int customParam)
        {
            // 弹幕数量 = 自定义参数 * 原弹幕数量
            var spreadModel_custom = this.spreadModel;
            if (spreadModel_custom.count != 0)
            {
                spreadModel_custom = spreadModel_custom.GetCustomModel(customParam);
            }

            return new GameActionModel_LaunchProjectile(
                projectileId,
                selector,
                preconditionSet,
                launchOffset,
                barrageType,
                customLaunchOffsetModel,
                spreadModel_custom
            );
        }

        public override string ToString()
        {
            return $"发射投射物行为: 投射物Id:{this.projectileId} 发射偏移:{this.launchOffset}";
        }

    }
}