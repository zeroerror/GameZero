using GamePlay.Bussiness.Logic;
using UnityEngine;

namespace GamePlay.Bussiness.Render
{
    public interface GameDirectorDomainApiR
    {
        /// <summary> 有限状态机接口 </summary>
        public GameDirectorFSMDomainApiR directorFSMApi { get; }

        /// <summary> 设置时间缩放 </summary>
        public void SetTimeScale(float timeScale);

        /// <summary>
        /// 镜头看向当前回合的区域位置
        /// </summary>
        public void LookAtRoundArea();

        /// <summary>
        /// 获取当前回合的区域位置
        /// </summary>
        public Vector2 GetRoundAreaPosition();

        /// <summary>
        /// 获取点击单位实体
        /// <para>clickWorldPos: 点击的世界坐标</para>
        /// 
        /// </summary>
        public GameEntityBase GetClickEntity(in Vector2 clickWorldPos);

        /// <summary>
        /// 将屏幕坐标转换为世界坐标
        /// <para>screenPos: 屏幕坐标</para>
        public Vector2 ScreenToWorldPos(in Vector2 screenPos);

        /// <summary>
        /// 根据Id参数获取实体
        /// <para>entityType: 实体类型</para>
        /// <para>entityId: 实体ID</para>
        /// </summary>
        public GameEntityBase FindEntity(GameEntityType entityType, int entityId);

        public GameEntityBase CreatePreviewUnit(GameItemUnitModel unitModel);

        /// <summary>
        /// 销毁预览单位
        /// <para>entity: 预览单位</para>
        /// </summary>
        public void DestroyPreviewUnit(GameEntityBase entity);
    }
}