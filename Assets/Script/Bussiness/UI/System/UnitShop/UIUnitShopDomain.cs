using GamePlay.Bussiness.Logic;
using GamePlay.Core;

namespace GamePlay.Bussiness.UI
{
    public class UIUnitShopDomain : UISystemDomainBase, UIUnitShopDomainApi
    {

        public UIUnitShopDomain()
        {
        }

        protected override void _BindEvents()
        {
            this._context.BindRC(GameDirectorRCCollection.RC_GAME_DIRECTOR_STATE_ENTER_FIGHT_PREPARING, this._OnStateEnterFightPreparing);
            this._context.BindRC(GameDirectorRCCollection.RC_GAME_DIRECTOR_BUY_UNIT, this._OnBuyUnit);
        }

        protected override void _UnbindEvents()
        {
            this._context.UnbindRC(GameDirectorRCCollection.RC_GAME_DIRECTOR_STATE_ENTER_FIGHT_PREPARING, this._OnStateEnterFightPreparing);
            this._context.UnbindRC(GameDirectorRCCollection.RC_GAME_DIRECTOR_BUY_UNIT, this._OnBuyUnit);
        }

        private void _OnStateEnterFightPreparing(object args)
        {
            var evArgs = (GameDirectorRCArgs_StateEnterFightPreparing)args;
            var buyableUnits = this._context.logicApi.directApi.GetBuyableUnits();
            var viewInput = new UIUnitShopMainViewInput { buyableUnits = buyableUnits };
            this.OpenUI<UIUnitShopMainView>(new UIViewInput(viewInput));
        }

        private void _OnBuyUnit(object args)
        {
            var evArgs = (GameDirectorRCArgs_BuyUnit)args;
            GameLogger.DebugLog($"刷新购买单位列表, 购买成功->{evArgs.model}, 金币->{evArgs.costGold}");
            // 确认开始
            var lcArgs = new GameLCArgs_PreparingConfirmExit();
            var ldirectApi = this._context.logicApi.directApi;
            ldirectApi.SubmitEvent(GameLCCollection.LC_GAME_PREPARING_CONFIRM_EXIT, lcArgs);
        }
    }
}