namespace GamePlay.Bussiness.Logic
{
    public class GameRoleAIState_Attack : GameRoleAIStateBase
    {
        public readonly GameRoleEntity role;

        public GameEntityBase targetEntity;

        public GameRoleAIState_Attack(GameRoleEntity role)
        {
            this.role = role;
        }

        public override void Clear()
        {
            base.Clear();
            targetEntity = null;
        }

    }
}