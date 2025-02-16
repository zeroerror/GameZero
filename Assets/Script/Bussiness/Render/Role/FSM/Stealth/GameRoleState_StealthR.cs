namespace GamePlay.Bussiness.Render
{
    public class GameRoleState_StealthR : GameRoleStateBaseR
    {
        public float duration;
        public GameRoleState_StealthR() { }

        public override void Clear()
        {
            base.Clear();
            this.duration = 0;
        }
    }
}