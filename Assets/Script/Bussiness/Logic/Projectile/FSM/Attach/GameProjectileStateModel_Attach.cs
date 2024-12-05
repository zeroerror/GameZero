using GameVec2 = UnityEngine.Vector2;
namespace GamePlay.Bussiness.Logic
{
    public struct GameProjectileStateModel_Attach
    {
        /// <summary> 附着类型 </summary>
        public readonly GameProjectileTargeterType attachType;
        /// <summary> 附着偏移 </summary>
        public readonly GameVec2 attachOffset;

        public GameProjectileStateModel_Attach(GameProjectileTargeterType attachType, GameVec2 attachOffset)
        {
            this.attachType = attachType;
            this.attachOffset = attachOffset;
        }
    }
}