using GamePlay.Core;

namespace GamePlay.Bussiness.Logic
{
    public class GameRoleStateDomain_Any : GameRoleStateDomainBase
    {
        public override bool CheckEnter(GameRoleEntity role, params object[] args)
        {
            return true;
        }

        public override void Enter(GameRoleEntity role, params object[] args)
        {
        }

        protected override void _Tick(GameRoleEntity role, float dt)
        {
            var stateModel = role.fsmCom.anyState;
            stateModel.stateTime += dt;
            this._TickKeyFrame(role, dt);
        }

        protected override GameRoleStateType _CheckExit(GameRoleEntity role)
        {
            var attrCom = role.attributeCom;
            if (attrCom.GetValue(GameAttributeType.HP) <= 0)
            {
                return GameRoleStateType.Dead;
            }

            return GameRoleStateType.None;
        }

        private void _TickKeyFrame(GameRoleEntity role, float dt)
        {
        }
    }

}