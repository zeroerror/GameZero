using GamePlay.Bussiness.Logic;

namespace GamePlay.Bussiness.Render
{
    public class GameSkillModelR
    {
        public readonly int typeId;
        public readonly GameSkillType skillType;
        public readonly string clipUrl;
        public readonly float clipLength;

        public GameSkillModelR(int typeId, GameSkillType skllType, string clipUrl, float clipLength)
        {
            this.typeId = typeId;
            this.skillType = skllType;
            this.clipUrl = clipUrl;
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