using GamePlay.Core;

namespace GamePlay.Bussiness.Renderer
{
    public class GameRoleStateDomain_MoveR : GameRoleStateDomainBaseR
    {
        public GameRoleStateDomain_MoveR() : base() { }

        public override bool CheckEnter(GameRoleEntityR entity, params object[] args)
        {
            return true;
        }

        public override void Enter(GameRoleEntityR entity, params object[] args)
        {
            GameLogger.Log($"MoveR enter");
            var factory = this._roleContext.factory;
            var animCom = entity.animCom;
            if (animCom.hasClip("move"))
            {
                animCom.Play("move");
            }
            else
            {
                var clip = factory.LoadAnimationClip(entity.idCom.typeId, "move");
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