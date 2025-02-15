using System.Collections.Generic;
using GamePlay.Core;
namespace GamePlay.Bussiness.Logic
{
    public interface GamePhysicsDomainApi
    {
        /// <summary>
        /// 创建物理
        /// </summary>
        /// <para> entity 实体</para>
        /// <para> colliderModel 碰撞模型</para>
        /// <para> isTrigger 是否是触发器</para>
        public void CreatePhysics(GameEntityBase entity, GameColliderModelBase colliderModel, bool isTrigger);

        /// <summary>
        /// 创建物理
        /// </summary>
        /// <para> entity 实体</para>
        /// <para> physicsCom 物理组件</para>
        /// <para> colliderModel 碰撞模型</para>
        /// <para> isTrigger 是否是触发器</para>
        public void CreatePhysics(GameEntityBase entity, GamePhysicsCom physicsCom, GameColliderModelBase colliderModel, bool isTrigger);

        /// <summary>
        /// 移除物理
        /// </summary>
        public void RemovePhysics(GameEntityBase entity);

        /// <summary>
        /// 检测Box与哪些实体重叠
        /// <para> colliderModel 碰撞体模型</para>
        /// <para> transformArgs 变换参数</para>
        /// </summary>
        public List<GameEntityBase> GetOverlapEntities(GameColliderModelBase colliderModel, GameTransformArgs transformArgs);
    }
}
