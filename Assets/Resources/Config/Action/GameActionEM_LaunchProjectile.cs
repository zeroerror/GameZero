using GamePlay.Bussiness.Logic;
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
        public GameProjectileBarrageEM_Spread spreadModelEM;

        public GameActionModel_LaunchProjectile ToModel()
        {
            GameActionModel_LaunchProjectile model = new GameActionModel_LaunchProjectile();
            model.projectileId = this.projectileId;
            model.launchOffset = this.launchOffset;
            model.barrageType = this.barrageType;
            switch (this.barrageType)
            {
                case GameProjectileBarrageType.CustomLaunchOffset:
                    model.customLaunchOffsetModel = this.customLaunchOffsetEM.ToModel();
                    break;
                case GameProjectileBarrageType.Spread:
                    model.spreadModel = this.spreadModelEM.ToModel();
                    break;
            }
            return model;
        }
    }
}