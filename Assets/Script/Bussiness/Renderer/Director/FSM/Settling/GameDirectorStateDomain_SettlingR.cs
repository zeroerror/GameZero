using GamePlay.Bussiness.Logic;

namespace GamePlay.Bussiness.Renderer
{
    public class GameDirectorStateDomain_SettlingR : GameDirectorStateDomainBaseR
    {
        public GameDirectorStateDomain_SettlingR(GameDirectorDomainR directorDomain) : base(directorDomain)
        {
        }

        public override void BindEvents()
        {
        }

        public override void UnbindEvents()
        {
        }

        public override void Enter(GameDirectorEntityR director, object args = null)
        {
        }

        protected override void _Tick(GameDirectorEntityR director, float frameTime)
        {
        }

        public override void ExitTo(GameDirectorEntityR director, GameDirectorStateType toState)
        {
        }

    }
}