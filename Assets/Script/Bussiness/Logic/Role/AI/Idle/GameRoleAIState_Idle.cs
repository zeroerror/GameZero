namespace GamePlay.Bussiness.Logic
{
    public class GameRoleAIState_Idle : GameRoleAIStateBase
    {
        GameRoleEntity role;

        public GameRoleAIState_Idle(GameRoleEntity role)
        {
            this.role = role;
        }

        public override void Clear()
        {
            base.Clear();
        }
    }
}