namespace GamePlay.Bussiness.UI
{
    public interface UIPlayerDomainApi
    {
        /// <summary>
        /// 检查金币是否足够
        /// <para>cost: 花费</para>
        /// </summary>
        public bool IsGoldEnough(int cost);

        /// <summary>
        /// 当前金币
        /// </summary>
        public int curGold { get; }
    }
}