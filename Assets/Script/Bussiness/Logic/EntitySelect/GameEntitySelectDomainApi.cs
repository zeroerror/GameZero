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


        /// <summary>
        /// 判断选择器锚点是否有效
        /// <para>会根据选择锚点类型以及行为方的目标选取器来判断是否存在有效的锚点</para>
        /// <para>比如锚点角色处于非存活状态，则锚点无效</para>
        /// <para>actor 执行者</para>
        /// <para>selector 选择器</para>
        /// </summary>
        public bool CheckSelectorAnchor(GameEntityBase actor, GameEntitySelector selector);
    }
}