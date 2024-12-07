using GameVec2 = UnityEngine.Vector2;
namespace GamePlay.Bussiness.Logic
{
    public struct GameProjectileBarrageModel_CustomLaunchOffset
    {
        public readonly int count;
        public readonly GameVec2[] launchOffsets;

        public GameProjectileBarrageModel_CustomLaunchOffset(int count, GameVec2[] launchOffsets)
        {
            this.count = count;
            this.launchOffsets = launchOffsets;
        }
    }
}