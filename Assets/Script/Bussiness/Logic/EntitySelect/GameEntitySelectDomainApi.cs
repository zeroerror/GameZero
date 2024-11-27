using System.Collections.Generic;
namespace GamePlay.Bussiness.Logic
{
    public interface GameEntitySelectDomainApi
    {
        /// <summary>
        /// 选择实体
        /// </summary>
        /// <returns></returns>
        public List<GameEntityBase> GetSelectdeEntities(GameEntitySelector selector, GameEntityBase actorEntity);
    }
}