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
    }
}