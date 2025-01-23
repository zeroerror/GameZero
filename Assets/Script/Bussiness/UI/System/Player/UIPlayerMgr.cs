namespace GamePlay.Bussiness.UI
{
    public class UIPlayerMgr : UIMgrBase<UIPlayerMgr>
    {
        public UIPlayerModel model { get; private set; }

        public UIPlayerMgr()
        {
            this.model = new UIPlayerModel();
        }

        /// <summary>
        /// 金币是否足够
        /// </summary>
        public bool IsGoldEnough(int gold)
        {
            return this.model.gold >= gold;
        }
    }
}