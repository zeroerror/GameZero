using GamePlay.Core;
using GameVec2 = UnityEngine.Vector2;

namespace GamePlay.Bussiness.Renderer
{
    public class GameActionModelR
    {
        public int typeId;
        public string prefabUrl;
        public GameVec2 scale = GameVec2.one;
        public GameVec2 offset = GameVec2.zero;

        public GameCameraShakeModel shakeModel;

        public GameActionModelR(
            int typeId,
            string prefabUrl,
            in GameVec2 scale,
            in GameVec2 offset,
            GameCameraShakeModel shakeModel
        )
        {
            this.typeId = typeId;
            this.prefabUrl = prefabUrl;
            this.scale = scale;
            this.offset = offset;
            this.shakeModel = shakeModel;
        }
    }
}