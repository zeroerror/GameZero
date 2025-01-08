using GamePlay.Bussiness.Logic;

namespace GamePlay.Bussiness.Renderer
{
    public interface GameActionDomainApiR
    {
        public bool TryGetModel(int actionId, out GameActionModelR model);
    }
}