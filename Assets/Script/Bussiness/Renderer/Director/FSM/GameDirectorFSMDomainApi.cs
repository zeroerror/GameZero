using GamePlay.Bussiness.Logic;

namespace GamePlay.Bussiness.Renderer
{
    public interface GameDirectorFSMDomainApiR
    {
        public void Enter(GameDirectorEntityR director, GameDirectorStateType state, object args = null);
    }
}