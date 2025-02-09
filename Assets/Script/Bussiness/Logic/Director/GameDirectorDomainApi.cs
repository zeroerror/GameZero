using GameVec2 = UnityEngine.Vector2;
namespace GamePlay.Bussiness.Logic
{
    public interface GameDirectorDomainApi
    {
        /// <summary> 有限状态机接口 </summary>
        public GameDirectorFSMDomainApi directorFSMApi { get; }

        /// <summary>
        /// 设置逻辑层的时间缩放
        /// </summary>
        public void SetTimeScale(float timeScale);

        /// <summary>
        /// 驱动渲染调用(RC)事件
        /// </summary>
        public void TickRCEvents();

        /// <summary>
        /// 提交事件
        /// </summary>
        public void SubmitEvent(string eventName, object args);

        /// <summary>
        /// 绑定RC事件
        /// </summary>
        public void BindRC(string rcName, System.Action<object> callback);

        /// <summary>
        /// 解绑RC事件
        /// </summary>
        public void UnbindRC(string rcName, System.Action<object> callback);

        /// <summary>
        /// 购买单位
        /// <para>index: 购买单位的索引</para>
        /// </summary>
        public void BuyUnit(int index);

        /// <summary>
        /// 创建单位
        /// <para>itemEntity: 单位实体</para>
        /// </summary>
        public GameEntityBase CreateUnit(GameUnitItemEntity itemEntity);

        /// <summary>
        /// 获取单位物品对应的实体
        /// <para>itemEntity: 单位实体</para>
        /// </summary>
        public GameEntityBase FindUnitEntity(GameUnitItemEntity itemEntity);

        /// <summary>
        /// 获取单位物品对应的实体
        /// <para>entityType: 实体类型</para>
        /// <para>entityId: 实体ID</para>
        /// </summary>
        public GameEntityBase FindUnitEntity(GameEntityType entityType, int entityId);

        /// <summary>
        /// 获取单位物品实体
        /// <para>entityType: 实体类型</para>
        /// <para>entityId: 实体ID</para>
        /// </summary>
        public GameUnitItemEntity FindUnitItemEntity(GameEntityType entityType, int entityId);

        /// <summary>
        /// 根据Id参数获取实体
        /// <para>idArgs: 实体ID参数</para>
        /// </summary>
        public GameEntityBase FindEntity(in GameIdArgs idArgs);

        /// <summary>
        /// 获取当前可购买单位列表
        /// </summary>
        public GameUnitItemModel[] GetBuyableUnits();

        /// <summary>
        /// 洗牌可购买单位列表
        /// <para>isFree: 是否是免费洗牌</para>
        /// </summary>
        public bool ShuffleBuyableUnits(bool isFree);

        /// <summary>
        /// 清理当前战场, 并移除当前所有单位
        /// </summary>
        public void CleanBattleField();

        /// <summary>
        /// 获取当前回合的区域位置
        /// </summary>
        public GameVec2 GetRoundAreaPosition();

        /// <summary>
        /// 当前回合数
        /// </summary>
        public int curRound { get; }
    }
}