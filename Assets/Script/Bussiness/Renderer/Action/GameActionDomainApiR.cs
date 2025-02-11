using GamePlay.Bussiness.Logic;

namespace GamePlay.Bussiness.Render
{
    public interface GameActionDomainApiR
    {
        public bool TryGetModel(int actionId, out GameActionModelR model);
    }
}