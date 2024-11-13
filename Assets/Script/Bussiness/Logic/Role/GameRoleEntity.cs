namespace GamePlay.Bussiness.Logic
{
    public class GameRoleEntity : GameEntityBase
    {
        public GameRoleFSMCom fsmCom { get; private set; }

        public GameRoleEntity() : base(0, GameEntityType.Role)
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