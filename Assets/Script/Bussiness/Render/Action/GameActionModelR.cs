using GamePlay.Core;
using GameVec2 = UnityEngine.Vector2;

namespace GamePlay.Bussiness.Render
{
    public class GameActionModelR
    {
        public int typeId;
        public string desc;

        public string actEffectUrl;
        public GameVec2 actVFXScale = GameVec2.one;
        public GameVec2 actVFXOffset = GameVec2.zero;
        public GameCameraShakeModel actCamShakeModel;

        public string hitEffectUrl;
        public GameVec2 hitVFXScale = GameVec2.one;
        public GameVec2 hitVFXOffset = GameVec2.zero;
        public GameCameraShakeModel hitCamShakeModel;

        public GameActionModelR(
            int typeId,
            string desc,
            string actEffectUrl,
            in GameVec2 actVFXScale,
            in GameVec2 actVFXOffset,
            GameCameraShakeModel actCamShakeModel,
            string hitEffectUrl,
            in GameVec2 hitVFXScale,
            in GameVec2 hitVFXOffset,
            GameCameraShakeModel hitCamShakeModel)
        {
            this.typeId = typeId;
            this.desc = desc;

            this.actEffectUrl = actEffectUrl;
            this.actVFXScale = actVFXScale;
            this.actVFXOffset = actVFXOffset;
            this.actCamShakeModel = actCamShakeModel;

            this.hitEffectUrl = hitEffectUrl;
            this.hitVFXScale = hitVFXScale;
            this.hitVFXOffset = hitVFXOffset;
            this.hitCamShakeModel = hitCamShakeModel;
        }
    }
}