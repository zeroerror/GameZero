using System.Collections.Generic;
using GameVec2 = UnityEngine.Vector2;
namespace GamePlay.Bussiness.Logic
{
    public interface GameEntitySelectDomainApi
    {
        /// <summary>
        /// 选择实体 
        /// <para>selector 选择器</para>
        /// <para>actorEntity 执行者</para>
        /// <para>ignoreRepeatCollision 是否忽略重复碰撞</para>
        /// </summary>
        public List<GameEntityBase> SelectEntities(GameEntitySelector selector, GameEntityBase actor, bool ignoreRepeatCollision = true);

        /// <summary>
        /// 获取选择器锚点位置
        /// <para>actor 执行者</para>
        /// <para>target 目标</para>
        /// <para>selector 选择器</para>
        /// </summary>
        public GameVec2 GetSelectorAnchorPosition(GameEntityBase actor, GameEntitySelector selector);
    }
}