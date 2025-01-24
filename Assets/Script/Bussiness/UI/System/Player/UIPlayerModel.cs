namespace GamePlay.Bussiness.UI
{
    public class UIPlayerModel
    {
        /// <summary> 持有金币 </summary>
        public int coins;

        public UIPlayerModel()
        {
        }

        public void Clear()
        {
            this.coins = 0;
        }
    }
}