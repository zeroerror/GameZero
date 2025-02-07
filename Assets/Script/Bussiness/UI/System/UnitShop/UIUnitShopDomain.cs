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
            this._context.BindRC(GameDirectorRCCollection.RC_GAME_DIRECTOR_STATE_ENTER_FIGHTING, this._OnStateEnterFighting);
            this._context.BindRC(GameDirectorRCCollection.RC_GAME_DIRECTOR_BUY_UNIT, this._OnBuyUnit);
        }

        protected override void _UnbindEvents()
        {
            this._context.UnbindRC(GameDirectorRCCollection.RC_GAME_DIRECTOR_STATE_ENTER_FIGHT_PREPARING, this._OnStateEnterFightPreparing);
            this._context.UnbindRC(GameDirectorRCCollection.RC_GAME_DIRECTOR_STATE_ENTER_FIGHTING, this._OnStateEnterFighting);
            this._context.UnbindRC(GameDirectorRCCollection.RC_GAME_DIRECTOR_BUY_UNIT, this._OnBuyUnit);
        }

        private void _OnStateEnterFightPreparing(object args)
        {
            var rcArgs = (GameDirectorRCArgs_StateEnterFightPreparing)args;
            var buyableUnits = this._context.logicApi.directorApi.GetBuyableUnits();
            var viewInput = new UIUnitShopMainViewInput { buyableUnits = buyableUnits };
            this.OpenUI<UIUnitShopMainView>(new UIViewInput(viewInput));
        }

        private void _OnStateEnterFighting(object args)
        {
            this.CloseUI<UIUnitShopMainView>();
        }

        private void _OnBuyUnit(object args)
        {
            var rcArgs = (GameDirectorRCArgs_BuyUnit)args;
            GameLogger.DebugLog($"刷新购买单位列表, 购买成功->{rcArgs.model}, 消耗金币->{rcArgs.costGold}");
        }
    }
}