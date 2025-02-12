namespace GamePlay.Bussiness.Logic
{
    public class GameRoleState_Stealth : GameRoleStateBase
    {
        public float duration;
        public GameRoleState_Stealth() { }

        public override void Clear()
        {
            base.Clear();
            this.duration = 0;
        }
    }
}