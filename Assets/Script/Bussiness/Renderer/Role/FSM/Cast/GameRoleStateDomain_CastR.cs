using GamePlay.Core;

namespace GamePlay.Bussiness.Renderer
{
    public class GameRoleStateDomain_CastR : GameRoleStateDomainBaseR
    {
        public GameRoleStateDomain_CastR() : base() { }

        public override bool CheckEnter(GameRoleEntityR entity, params object[] args)
        {
            return true;
        }

        public override void Enter(GameRoleEntityR entity, params object[] args)
        {
            GameLogger.Log($"CastR enter");
            var factory = this._roleContext.factory;
            var animCom = entity.animCom;
            if (animCom.hasClip("cast"))
            {
                animCom.Play("cast");
            }
            else
            {
                var clip = factory.LoadAnimationClip(entity.idCom.typeId, "cast");
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