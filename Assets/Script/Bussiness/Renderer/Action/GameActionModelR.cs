using GamePlay.Core;
using GameVec2 = UnityEngine.Vector2;

namespace GamePlay.Bussiness.Renderer
{
    public class GameActionModelR
    {
        public int typeId;
        public string vfxPrefabUrl;
        public GameVec2 vfxScale = GameVec2.one;
        public GameVec2 vfxOffset = GameVec2.zero;

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
            this.vfxPrefabUrl = prefabUrl;
            this.vfxScale = scale;
            this.vfxOffset = offset;
            this.shakeModel = shakeModel;
        }
    }
}