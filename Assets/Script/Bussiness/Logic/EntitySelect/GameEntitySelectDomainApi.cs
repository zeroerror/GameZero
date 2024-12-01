using System.Collections.Generic;
namespace GamePlay.Bussiness.Logic
{
    public interface GameEntitySelectDomainApi
    {
        /// <summary>
        /// 选择实体 
        /// <para>selector 选择器</para>
        /// <para>actorEntity 执行者实体</para>
        /// <para>ignoreRepeatCollision 是否忽略重复碰撞</para>
        /// </summary>
        public List<GameEntityBase> SelectEntities(GameEntitySelector selector, GameEntityBase actorEntity, bool ignoreRepeatCollision = true);
    }
}