namespace GamePlay.Bussiness.Logic
{
    public class GameSkillModel
    {
        public readonly int typeId;
        public readonly string animName;
        public readonly int totalFrame;

        public GameSkillModel(int typeId, string animName, int totalFrame)
        {
            this.typeId = typeId;
            this.animName = animName;
            this.totalFrame = totalFrame;
        }
    }
}