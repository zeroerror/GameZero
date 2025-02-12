using GamePlay.Bussiness.Logic;

namespace GamePlay.Bussiness.Render
{
    public interface GameEntityCollectDomainApiR
    {
        /// <summary>
        /// 直接回收实体
        /// </summary>
        /// <param name="entity"></param>
        public void CollectEntity(GameEntityBase entity);
    }
}