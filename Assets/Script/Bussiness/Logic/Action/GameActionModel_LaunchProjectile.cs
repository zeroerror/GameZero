using GameVec2 = UnityEngine.Vector2;
namespace GamePlay.Bussiness.Logic
{
    public class GameActionModel_LaunchProjectile : GameActionModelBase
    {
        public int projectileId;
        public GameVec2 launchOffset;

        public GameProjectileBarrageType barrageType;
        public GameProjectileBarrageModel_CustomLaunchOffset customLaunchOffsetModel;
        public GameProjectileBarrageModel_Spread spreadModel;

        public override string ToString()
        {
            return $"发射投射物行为: 投射物Id:{this.projectileId} 发射偏移:{this.launchOffset}";
        }

    }
}