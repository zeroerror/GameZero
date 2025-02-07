using GamePlay.Bussiness.Logic;

namespace GamePlay.Bussiness.UI
{
    public class UIPlayerDomain : UISystemDomainBase, UIPlayerDomainApi
    {
        public UIPlayerModel model { get; private set; }

        public UIPlayerDomain()
        {
            this.model = new UIPlayerModel();
        }

        protected override void _BindEvents()
        {
            this._context.BindRC(GameDirectorRCCollection.RC_GAME_DIRECTOR_COINS_CHANGE, this._OnCoinsChange);
        }

        protected override void _UnbindEvents()
        {
            this._context.UnbindRC(GameDirectorRCCollection.RC_GAME_DIRECTOR_COINS_CHANGE, this._OnCoinsChange);
        }

        private void _OnCoinsChange(object args)
        {
            var evArgs = (GameDirectorRCArgs_GoldChange)args;
            this.model.gold = evArgs.gold;
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