using UnityEngine;
using GameVec2 = UnityEngine.Vector2;
namespace GamePlay.Bussiness.Renderer
{
    public class GameProjectileModelR
    {
        public readonly int typeId;
        public readonly AnimationClip animClip;
        public readonly string prefabUrl;
        public readonly GameVec2 prefabScale;
        public readonly GameVec2 prefabOffset;
        public readonly bool isLockRotation;

        public GameProjectileModelR(
            int typeId,
            AnimationClip animClip,
            string prefabUrl,
            in GameVec2 prefabScale,
            in GameVec2 prefabOffset,
            bool isLockRotation
        )
        {
            this.typeId = typeId;
            this.animClip = animClip;
            this.prefabUrl = prefabUrl;
            this.prefabScale = prefabScale;
            this.prefabOffset = prefabOffset;
            this.isLockRotation = isLockRotation;
        }
    }
}