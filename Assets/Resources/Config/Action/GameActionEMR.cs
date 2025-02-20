using UnityEngine;
using GameVec2 = UnityEngine.Vector2;
using GamePlay.Bussiness.Render;
using GamePlay.Core;
using GamePlay.Infrastructure;

namespace GamePlay.Config
{
    [System.Serializable]
    public class GameActionEMR
    {
        public int typeId;
        public string desc;

        public AudioClip actSFX;
        public float actSFXVolume;
        public GameObject actVFX;
        public GameVec2 actVFXScale = GameVec2.one;
        public GameVec2 actVFXOffset = GameVec2.zero;
        public GameCameraShakeModel actCamShakeModel;

        public AudioClip hitSFX;
        public float hitSFXVolume;
        public GameObject hitVFX;
        public GameVec2 hitVFXScale = GameVec2.one;
        public GameVec2 hitVFXOffset = GameVec2.zero;
        public GameCameraShakeModel hitCamShakeModel;

        public GameActionModelR ToModel()
        {
            if (actVFX && (actVFXScale.x == 0 || actVFXScale.y == 0))
            {
                GameLogger.LogWarning("请注意 行为特效缩放值为0");
            }

            if (hitVFX && (hitVFXScale.x == 0 || hitVFXScale.y == 0))
            {
                GameLogger.LogWarning("请注意 受击特效缩放值为0");
            }

            return new GameActionModelR(
                this.typeId,
                this.desc,
                this.actSFX?.GetAssetUrl(),
                this.actSFXVolume,
                this.actVFX?.GetAssetUrl(),
                this.actVFXScale,
                this.actVFXOffset,
                this.actCamShakeModel?.amplitude != 0 ? this.actCamShakeModel : null,
                this.hitSFX?.GetAssetUrl(),
                this.hitSFXVolume,
                this.hitVFX?.GetAssetUrl(),
                this.hitVFXScale,
                this.hitVFXOffset,
                this.hitCamShakeModel?.amplitude != 0 ? this.hitCamShakeModel : null
            );
        }

    }
}