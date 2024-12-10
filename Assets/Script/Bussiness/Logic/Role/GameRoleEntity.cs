using GamePlay.Core;
using GameVec2 = UnityEngine.Vector2;
namespace GamePlay.Bussiness.Logic
{
    public class GameRoleEntity : GameEntityBase
    {
        public GameRoleModel model { get; private set; }
        public GameRoleInputCom inputCom { get; private set; }
        public GameRoleFSMCom fsmCom { get; private set; }
        public GameSkillComp skillCom { get; private set; }

        public GameRoleAICom aiCom { get; private set; }

        public GameRoleEntity(GameRoleModel model) : base(model.typeId, GameEntityType.Role)
        {
            this.model = model;
            this.SetByModel(model);
            this.inputCom = new GameRoleInputCom();
            this.fsmCom = new GameRoleFSMCom();
            this.skillCom = new GameSkillComp(this);
            this.aiCom = new GameRoleAICom();
        }

        public override void Clear()
        {
            base.Clear();
            this.SetByModel(this.model);
        }

        public void SetByModel(GameRoleModel model)
        {
            if (model == null) return;
            model.attributes?.Foreach((value, index) =>
            {
                this.attributeCom.SetAttribute(value);
                this.baseAttributeCom.SetAttribute(value);
            });
        }

        public override void Tick(float dt)
        {
        }

        public override void Destroy()
        {
        }

        public void FaceTo(in GameVec2 dir)
        {
            this.transformCom.forward = dir;
            if (dir.x != 0)
            {
                var scale = this.transformCom.scale;
                var absx = GameMathF.Abs(scale.x);
                scale.x = dir.x > 0 ? absx : -absx;
                this.transformCom.scale = scale;
            }
        }

    }
}