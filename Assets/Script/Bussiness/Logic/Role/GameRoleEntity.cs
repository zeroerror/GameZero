namespace GamePlay.Bussiness.Logic
{
    public class GameRoleEntity : GameEntityBase
    {
        public GameRoleInputCom inputCom { get; private set; }
        public GameRoleFSMCom fsmCom { get; private set; }
        public GameSkillComp skillCom { get; private set; }

        public GameRoleEntity(int typeId) : base(typeId, GameEntityType.Role)
        {
            this.inputCom = new GameRoleInputCom();
            this.fsmCom = new GameRoleFSMCom();
            this.skillCom = new GameSkillComp(this);
        }

        public override void Tick(float dt)
        {
        }
    }
}