using System.Collections.Generic;
namespace GamePlay.Bussiness.Logic
{
    public interface GameEntitySelectDomainApi
    {
        /// <summary>
        /// 选择实体 
        /// <para>selector 选择器</para>
        /// <para>actorEntity 执行者实体</para>
        /// </summary>
        public List<GameEntityBase> GetSelectdeEntities(GameEntitySelector selector, GameEntityBase actorEntity);
    }
}