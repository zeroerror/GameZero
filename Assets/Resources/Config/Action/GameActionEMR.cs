using GamePlay.Bussiness.Renderer;
using GamePlay.Core;
using UnityEngine;
using GameVec2 = UnityEngine.Vector2;

namespace GamePlay.Config
{
    [System.Serializable]
    public class GameActionEMR
    {
        public int typeId;

        public GameObject vfxPrefab;
        public GameVec2 vfxScale = GameVec2.one;
        public GameVec2 vfxOffset = GameVec2.zero;

        public GameCameraShakeModel camShakeModel;

        public GameActionModelR ToModel()
        {
            var url = this.vfxPrefab?.GetPrefabUrl();
            if (vfxScale.x == 0 || vfxScale.y == 0)
            {
                GameLogger.LogWarning("请注意 行为特效缩放值为0");
            }
            return new GameActionModelR(
                this.typeId,
                url,
                this.vfxScale,
                this.vfxOffset,
                this.camShakeModel
            );
        }

    }
}