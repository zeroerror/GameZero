namespace GamePlay.Bussiness.Renderer
{
    public class GameRoleState_CastR : GameRoleStateBaseR
    {
        public GameSkillEntityR skill;
        public GameRoleState_CastR() { }

        public override void Clear()
        {
            base.Clear();
            this.skill = null;
        }
    }
}