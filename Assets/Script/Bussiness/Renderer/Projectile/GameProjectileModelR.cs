using UnityEngine;
namespace GamePlay.Bussiness.Renderer
{
    public class GameProjectileModel
    {
        public readonly int typeId;
        public readonly AnimationClip animClip;

        public GameProjectileModel(int typeId, AnimationClip animClip)
        {
            this.typeId = typeId;
            this.animClip = animClip;
        }
    }
}