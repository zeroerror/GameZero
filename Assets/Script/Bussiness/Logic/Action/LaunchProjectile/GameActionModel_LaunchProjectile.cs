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
            int typeId,
            int projectileId,
            GameEntitySelector selector,
            GameActionPreconditionSetModel preconditionSet,
            in GameVec2 randomValueOffset,
            in GameVec2 launchOffset,
            GameProjectileBarrageType barrageType,
            GameProjectileBarrageModel_CustomLaunchOffset customLaunchOffsetModel,
            GameProjectileBarrageModel_Spread spreadModel
        ) : base(GameActionType.LaunchProjectile, typeId, selector, preconditionSet, randomValueOffset)
        {
            this.projectileId = projectileId;
            this.launchOffset = launchOffset;
            this.barrageType = barrageType;
            this.customLaunchOffsetModel = customLaunchOffsetModel;
            this.spreadModel = spreadModel;
        }

        public override GameActionModelBase GetCustomModel(float customParam)
        {
            // 弹幕数量 = 自定义参数 * 原弹幕数量
            var spreadModel_custom = this.spreadModel;
            if (spreadModel_custom.count != 0)
            {
                spreadModel_custom = spreadModel_custom.GetCustomModel(customParam);
            }

            return new GameActionModel_LaunchProjectile(
                this.typeId,
                this.projectileId,
                this.selector,
                this.preconditionSet,
                this.randomValueOffset,
                this.launchOffset,
                this.barrageType,
                this.customLaunchOffsetModel,
                spreadModel_custom
            );
        }

        public override string ToString()
        {
            return $"发射投射物行为: 投射物Id:{this.projectileId} 发射偏移:{this.launchOffset}";
        }

    }
}