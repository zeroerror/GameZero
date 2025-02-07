namespace GamePlay.Bussiness.UI
{
    public class UIPlayerModel
    {
        /// <summary> 持有金币 </summary>
        public int gold;

        public UIPlayerModel()
        {
        }

        public void Clear()
        {
            this.gold = 0;
        }
    }
}