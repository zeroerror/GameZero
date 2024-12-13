using GamePlay.Bussiness.Logic;
using UnityEngine;

namespace GamePlay.Bussiness.Renderer
{
    public class GameSkillModelR
    {
        public readonly int typeId;
        public readonly GameSkillType skillType;
        public readonly AnimationClip animClip;

        public GameSkillModelR(int typeId, GameSkillType skllType, AnimationClip animClip)
        {
            this.typeId = typeId;
            this.skillType = skllType;
            this.animClip = animClip;
        }
        /// <summary> 是否受到攻速的影响 </summary>
        public bool effectByAttackSpeed
        {
            get
            {
                return this.skillType == GameSkillType.NormalAttack;
            }
        }
    }
}