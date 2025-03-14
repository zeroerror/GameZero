using GamePlay.Core;
using GamePlay.Infrastructure;
using GameVec2 = UnityEngine.Vector2;
namespace GamePlay.Bussiness.Logic
{
    public class GameRoleEntity : GameEntityBase
    {
        public GameRoleModel model { get; private set; }

        public GameRoleInputCom inputCom { get; private set; }
        public GameRoleAICom aiCom { get; private set; }
        public GameRoleFSMCom fsmCom { get; private set; }

        public GameSkillCom skillCom { get; private set; }

        public GameBuffCom buffCom { get; private set; }

        public GamePhysicsCom colliderPhysicsCom { get; private set; }

        public GameRoleEntity(GameRoleModel model) : base(model.typeId, GameEntityType.Role)
        {
            this.model = model;
            this.SetByModel(model);
            this.inputCom = new GameRoleInputCom();
            this.aiCom = new GameRoleAICom(this);
            this.fsmCom = new GameRoleFSMCom();
            this.skillCom = new GameSkillCom(this);
            this.buffCom = new GameBuffCom();
            this.colliderPhysicsCom = new GamePhysicsCom();
        }

        public override void Clear()
        {
            base.Clear();
            this.inputCom.Clear();
            this.SetByModel(this.model);
            this.skillCom.Clear();
        }

        public override void SetInvalid()
        {
            base.SetInvalid();
            this.skillCom.ForeachSkills((skill) =>
            {
                skill.SetInvalid();
            });
            this.SetPhysicsEnable(false);
        }

        public override void SetValid()
        {
            base.SetValid();
            this.SetPhysicsEnable(true);
        }

        public void SetPhysicsEnable(bool enable)
        {
            var collider = this.physicsCom.collider;
            if (collider != null) collider.isEnable = enable;
            var collider2 = this.colliderPhysicsCom.collider;
            if (collider2 != null) collider2.isEnable = enable;
        }

        public override bool IsAlive()
        {
            if (!base.IsAlive()) return false;
            var stateType = this.fsmCom.stateType;
            return stateType != GameRoleStateType.Dead;
        }

        public void SetByModel(GameRoleModel model)
        {
            if (model == null) return;
            model.attributes?.Foreach((value, index) =>
            {
                this.attributeCom.SetAttribute(value);
                this.baseAttributeCom.SetAttribute(value);

                // 保证最大生命值同步
                if (value.type == GameAttributeType.MaxHP)
                {
                    var attribute = new GameAttribute { type = GameAttributeType.HP, value = value.value };
                    this.attributeCom.SetAttribute(attribute);
                }
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
            if (dir.x == 0) return;
            var scale = this.transformCom.scale;
            var absx = GameMathF.Abs(scale.x);
            scale.x = dir.x > 0 ? absx : -absx;
            this.transformCom.scale = scale;
            this._forward = dir;
        }
        public GameVec2 forward => this._forward;
        private GameVec2 _forward;
    }
}