using GamePlay.Bussiness.Logic;
using JetBrains.Annotations;
using GameVec2 = UnityEngine.Vector2;
namespace GamePlay.Config
{
    [System.Serializable]
    public class GameActionEM_LaunchProjectile
    {
        public int projectileId;
        public GameVec2 launchOffset;
        public GameProjectileSO launchProjectileSO;

        public GameProjectileBarrageType barrageType;
        public GameProjectileBarrageEM_CustomLaunchOffset customLaunchOffsetEM;
        public GameProjectileBarrageEM_Spread spreadEM;

        public GameEntitySelectorEM selectorEM;


        public GameActionModel_LaunchProjectile ToModel()
        {
            GameProjectileBarrageModel_CustomLaunchOffset customLaunchOffsetModel = default;
            GameProjectileBarrageModel_Spread spreadModel = default;
            switch (this.barrageType)
            {
                case GameProjectileBarrageType.CustomLaunchOffset:
                    customLaunchOffsetModel = this.customLaunchOffsetEM.ToModel();
                    break;
                case GameProjectileBarrageType.Spread:
                    spreadModel = this.spreadEM.ToModel();
                    break;
            }

            GameActionModel_LaunchProjectile model = new GameActionModel_LaunchProjectile(
                this.projectileId,
                this.launchOffset,
                this.barrageType,
                customLaunchOffsetModel,
                spreadModel,
                this.selectorEM.ToSelector()
            );
            return model;
        }
    }
}