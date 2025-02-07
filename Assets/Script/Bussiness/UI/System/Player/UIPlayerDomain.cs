using GamePlay.Bussiness.Logic;

namespace GamePlay.Bussiness.UI
{
    public class UIPlayerDomain : UISystemDomainBase, UIPlayerDomainApi
    {
        private UIPlayerModel _model;

        /// <summary> 当前金币 </summary>
        public int curGold => this._model.gold;

        public UIPlayerDomain()
        {
            this._model = new UIPlayerModel();
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
            var rcArgs = (GameDirectorRCArgs_GoldChange)args;
            this._model.gold = rcArgs.gold;
            // 提交金币变化事件
            this._context.eventService.Submit(UIPlayerEventCollection.UI_PLAYER_COINS_CHANGE, rcArgs);
        }

        /// <summary>
        /// 金币是否足够
        /// </summary>
        public bool IsGoldEnough(int gold)
        {
            return this._model.gold >= gold;
        }

    }
}