using GamePlay.Bussiness.Logic;
using GamePlay.Core;

namespace GamePlay.Bussiness.Renderer
{
    public class GameRoleStateDomain_IdleR : GameRoleStateDomainBaseR
    {
        public GameRoleStateDomain_IdleR() : base() { }

        public override void BindEvents()
        {
            base.BindEvents();
            this._context.RegistRC(GameRoleRCCollection.RC_GAME_ROLE_STATE_ENTER_IDLE, this._OnEnter);
        }

        public override void UnbindEvents()
        {
            base.UnbindEvents();
            this._context.UnregistRC(GameRoleRCCollection.RC_GAME_ROLE_STATE_ENTER_IDLE, this._OnEnter);
        }

        private void _OnEnter(object args)
        {
            var evArgs = (GameRoleRCCollection.GameRoleRCArgs_StateEnterIdle)args;
            ref var idArgs = ref evArgs.idArgs;
            var role = this._roleContext.repo.FindByEntityId(idArgs.entityId);
            if (role == null)
            {
                this._context.delayRCEventService.Submit(GameRoleRCCollection.RC_GAME_ROLE_CREATE, args);
                return;
            }
            this.Enter(role);
        }


        public override bool CheckEnter(GameRoleEntityR entity, params object[] args)
        {
            return true;
        }

        public override void Enter(GameRoleEntityR entity, params object[] args)
        {
            GameLogger.Log($"IdleR enter");
            var factory = this._roleContext.factory;
            var animCom = entity.animCom;
            if (animCom.hasClip("idle"))
            {
                animCom.Play("idle");
            }
            else
            {
                var clip = factory.LoadAnimationClip(entity.idCom.typeId, "idle");
                animCom.Play(clip);
            }
        }

        protected override GameRoleStateType _CheckExit(GameRoleEntityR entity)
        {
            return GameRoleStateType.None;
        }

        protected override void _Tick(GameRoleEntityR entity, float frameTime)
        {
        }
    }
}