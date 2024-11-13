namespace GamePlay.Bussiness.Logic
{
    public class GameRoleEntity : GameEntity
    {
        public GameRoleFSMCom fsmCom { get; private set; }

        public GameRoleEntity()
        {
            this.fsmCom = new GameRoleFSMCom(this);
        }

        public override void Tick(float dt)
        {
            this.fsmCom.Tick(dt);
        }

        public override void Reset(float dt) { }
    }
}