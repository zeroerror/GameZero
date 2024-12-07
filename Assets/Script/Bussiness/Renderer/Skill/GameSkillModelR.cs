using UnityEngine;

namespace GamePlay.Bussiness.Renderer
{
    public class GameSkillModelR
    {
        public readonly int typeId;
        public readonly AnimationClip animClip;

        public GameSkillModelR(int typeId, AnimationClip animClip)
        {
            this.typeId = typeId;
            this.animClip = animClip;
        }
    }
}