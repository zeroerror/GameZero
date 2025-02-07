using GamePlay.Core;
using GameVec2 = UnityEngine.Vector2;
namespace GamePlay.Bussiness.Renderer
{
    public class GameDirectorStateDomain_LoadingR : GameDirectorStateDomainBaseR
    {
        public GameDirectorStateDomain_LoadingR(GameDirectorDomainR directorDomain) : base(directorDomain)
        {
        }

        public override void Enter(GameDirectorEntityR director, object args = null)
        {
        }

        protected override void _Tick(GameDirectorEntityR director, float frameTime)
        {
        }

    }
}