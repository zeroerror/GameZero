namespace GamePlay.Bussiness.Logic
{
    public class GameRoleState_Cast : GameRoleStateBase
    {
        public GameSkillEntity skill;

        public GameRoleState_Cast()
        {
        }

        public override void Clear()
        {
            base.Clear();
            skill = null;
        }

        public bool isOver()
        {
            return !this.skill.timelineCom.isPlaying;
        }
    }
}