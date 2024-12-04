using GamePlay.Bussiness.Logic;
using UnityEngine;
using GameVec2 = UnityEngine.Vector2;
namespace GamePlay.Config
{
    [System.Serializable]
    public struct GameProjectileStateEM_Attach
    {
        [Header("附着类型")]
        public GameProjectileTargeterType attachType;
        [Header("附着偏移")]
        public GameVec2 attachOffset;

        public GameProjectileStateModel_Attach ToModel()
        {
            return new GameProjectileStateModel_Attach(attachType, attachOffset);
        }
    }
}