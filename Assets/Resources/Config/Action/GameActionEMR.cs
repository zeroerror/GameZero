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

        public GameObject actEffectPrefab;
        public GameVec2 actVFXScale = GameVec2.one;
        public GameVec2 actVFXOffset = GameVec2.zero;
        public GameCameraShakeModel actCamShakeModel;

        public GameObject hitEffectPrefab;
        public GameVec2 hitVFXScale = GameVec2.one;
        public GameVec2 hitVFXOffset = GameVec2.zero;
        public GameCameraShakeModel hitCamShakeModel;

        public GameActionModelR ToModel()
        {
            if (actVFXScale.x == 0 || actVFXScale.y == 0)
            {
                GameLogger.LogWarning("请注意 行为特效缩放值为0");
            }
            return new GameActionModelR(
                this.typeId,
                this.actEffectPrefab?.GetPrefabUrl(),
                this.actVFXScale,
                this.actVFXOffset,
                this.actCamShakeModel?.amplitude != 0 ? this.actCamShakeModel : null,
                this.hitEffectPrefab?.GetPrefabUrl(),
                this.hitVFXScale,
                this.hitVFXOffset,
                this.hitCamShakeModel?.amplitude != 0 ? this.hitCamShakeModel : null
            );
        }

    }
}