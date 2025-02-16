using GamePlay.Bussiness.Logic;
using UnityEngine;

namespace GamePlay.Bussiness.Render
{
    public class GameSkillModelR
    {
        public readonly int typeId;
        public readonly GameSkillType skillType;
        public readonly string clipName;
        public readonly float clipLength;

        public GameSkillModelR(int typeId, GameSkillType skllType, string clipUrl, float clipLength)
        {
            this.typeId = typeId;
            this.skillType = skllType;
            this.clipName = clipUrl;
            this.clipLength = clipLength;
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