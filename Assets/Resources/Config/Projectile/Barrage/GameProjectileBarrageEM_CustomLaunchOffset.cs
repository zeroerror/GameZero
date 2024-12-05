using GamePlay.Bussiness.Logic;
using GameVec2 = UnityEngine.Vector2;
namespace GamePlay.Config
{
    public class GameProjectileBarrageEM_CustomLaunchOffset
    {
        public int count;
        public GameVec2[] launchOffsets;

        public GameProjectileBarrageModel_CustomLaunchOffset ToModel()
        {
            GameProjectileBarrageModel_CustomLaunchOffset model = new GameProjectileBarrageModel_CustomLaunchOffset(
                this.count,
                this.launchOffsets
            );
            return model;

        }
    }
}