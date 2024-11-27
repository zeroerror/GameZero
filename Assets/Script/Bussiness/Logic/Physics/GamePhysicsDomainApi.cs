using System.Collections.Generic;
using GamePlay.Core;
namespace GamePlay.Bussiness.Logic
{
    public interface GamePhysicsDomainApi
    {
        public void CreatePhysics(GameEntityBase entity, GameColliderModelBase colliderModel);
        public void RemovePhysics(GameEntityBase entity);

        /// <summary>
        /// 检测Box与哪些实体重叠
        /// colliderModel：碰撞体模型
        /// transformArgs：变换参数
        /// </summary>
        public List<GameEntityBase> GetOverlapEntities(GameColliderModelBase colliderModel, GameTransformArgs transformArgs);
    }
}
