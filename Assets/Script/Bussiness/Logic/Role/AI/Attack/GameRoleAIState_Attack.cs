namespace GamePlay.Bussiness.Logic
{
    public class GameRoleAIState_Attack : GameRoleAIStateBase
    {
        public GameEntityBase targetEntity;

        public GameRoleAIState_Attack() { }

        public override void Clear()
        {
            base.Clear();
            targetEntity = null;
        }

    }
}