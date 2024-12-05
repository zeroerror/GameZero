using GamePlay.Bussiness.Logic;
using UnityEngine;
using GameVec2 = UnityEngine.Vector2;
namespace GamePlay.Config
{
    [System.Serializable]
    public class GameProjectileBarrageEM_CustomLaunchOffset
    {
        [Header("发射数量")]
        public int count;
        [Header("发射偏移")]
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