using System.Collections.Generic;

namespace GamePlay.Bussiness.Renderer
{
    public interface GameBuffDomainApiR
    {
        public bool TryGetBuffModel(int buffId, out GameBuffModelR buffModel);
        public List<GameBuffModelR> GetBuffModelList();
    }
}