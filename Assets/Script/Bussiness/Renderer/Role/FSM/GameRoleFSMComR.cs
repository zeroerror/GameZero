using GamePlay.Core;
using GamePlay.Bussiness.Logic;
namespace GamePlay.Bussiness.Renderer
{
    public class GameRoleFSMComR : GameFSMCom
    {
        public GameRoleEntityR role { get; private set; }

        public GameRoleFSMComR(GameRoleEntityR role) : base()
        {
            this.role = role;
            this._init();
        }

        private void _init()
        {
            var idleState = new GameRoleState_Idle();
            this.CreateState(idleState);
            this.SetTransition(new GameRoleStateTransition_Idle2Move());
            this.SetTransition(new GameRoleStateTransition_Idle2Cast());
            this.SetTransition(new GameRoleStateTransition_Idle2Dead());


            var moveState = new GameRoleState_Move();
            this.CreateState(moveState);
            this.SetTransition(new GameRoleStateTransition_Move2Idle());
            this.SetTransition(new GameRoleStateTransition_Move2Cast());

            var castState = new GameRoleState_Cast();
            this.CreateState(castState);
            this.SetTransition(new GameRoleStateTransition_Cast2Idle());
            this.SetTransition(new GameRoleStateTransition_Cast2Move());

            var deadState = new GameRoleState_Dead();
            this.CreateState(deadState);

            this.SetAnyTransition(new GameRoleStateTransition_Any2Dead());
        }
    }
}

