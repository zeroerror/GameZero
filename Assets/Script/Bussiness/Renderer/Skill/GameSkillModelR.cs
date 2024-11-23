namespace GamePlay.Bussiness.Renderer
{
    public class GameSkillModelR
    {
        public readonly int typeId;
        public readonly string animName;
        public readonly float animLength;

        public GameSkillModelR(int typeId, string animName, float animLength)
        {
            this.typeId = typeId;
            this.animName = animName;
            this.animLength = animLength;
        }
    }
}