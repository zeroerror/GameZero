namespace GamePlay.Bussiness.Logic
{
    public class GameRoleStateModel_Cast : GameRoleStateModelBase
    {
        public GameSkillEntity skill;

        public GameRoleStateModel_Cast() { }

        public override void Clear()
        {
            base.Clear();
            skill = null;
        }
    }
}