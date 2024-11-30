using Unity.VisualScripting;
using GameVec2 = UnityEngine.Vector2;
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

        public void FaceTo(in GameVec2 dir)
        {
            this.transformCom.forward = dir;
            if (dir.x != 0)
            {
                this.transformCom.scale = dir.x > 0 ? 1 : -1;
            }
        }
    }
}