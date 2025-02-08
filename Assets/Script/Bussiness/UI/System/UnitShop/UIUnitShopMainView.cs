using GamePlay.Bussiness.Core;
using GamePlay.Bussiness.Logic;
using GamePlay.Core;
using UnityEngine;
using UnityEngine.UI;
namespace GamePlay.Bussiness.UI
{
    public struct UIUnitShopMainViewInput
    {
        public GameUnitItemModel[] buyableUnits;

        public UIUnitShopMainViewInput(GameUnitItemModel[] itemModels)
        {
            this.buyableUnits = itemModels;
        }
    }

    public class UIUnitShopMainView : UIBase
    {
        public override UILayerType layerType => UILayerType.Main;
        public override string uiPkgUrl => "UI/System/UnitShop";
        public override string uiName => "UIUnitShopMainView";
        public UIUnitShopMainViewBinder uiBinder;

        private UIUnitShopMainViewInput _viewInput;
        private GameUnitItemModel[] _itemModels => this._viewInput.buyableUnits;

        protected override void _OnInit()
        {
            this.uiBinder = new UIUnitShopMainViewBinder(this.go);
            this._viewInput = (UIUnitShopMainViewInput)this._uiInput.customData;
        }

        protected override void _BindEvents()
        {
            base._BindEvents();
            this._uiApi.directorApi.BindKeyAction(KeyCode.G, () => this._OnClickItem(0));
            this._uiApi.directorApi.BindKeyAction(KeyCode.H, () => this._OnClickItem(1));
            this._uiApi.directorApi.BindKeyAction(KeyCode.J, () => this._OnClickItem(2));
            this._uiApi.directorApi.BindKeyAction(KeyCode.K, () => this._OnClickItem(3));
            this._uiApi.directorApi.BindKeyAction(KeyCode.L, () => this._OnClickItem(4));
            this._uiApi.directorApi.BindKeyAction(KeyCode.Return, this._OnBtnConfirmClick);
            this._uiApi.directorApi.BindEvent(UIPlayerEventCollection.UI_PLAYER_COINS_CHANGE, this._OnPlayerCoinsChange);
        }

        protected override void _UnbindEvents()
        {
            base._UnbindEvents();
            this._uiApi.directorApi.UnbindKeyAction(KeyCode.G, () => this._OnClickItem(0));
            this._uiApi.directorApi.UnbindKeyAction(KeyCode.H, () => this._OnClickItem(1));
            this._uiApi.directorApi.UnbindKeyAction(KeyCode.J, () => this._OnClickItem(2));
            this._uiApi.directorApi.UnbindKeyAction(KeyCode.K, () => this._OnClickItem(3));
            this._uiApi.directorApi.UnbindKeyAction(KeyCode.L, () => this._OnClickItem(4));
            this._uiApi.directorApi.UnbindKeyAction(KeyCode.Return, this._OnBtnConfirmClick);
            this._uiApi.directorApi.UnbindEvent(UIPlayerEventCollection.UI_PLAYER_COINS_CHANGE, this._OnPlayerCoinsChange);
        }

        private void _OnPlayerCoinsChange(object args)
        {
            this._refreshGold();
        }

        protected override void _OnShow()
        {
            this._refreshGold();

            var itemCount = this._itemModels.Length;
            for (var i = 0; i < 5; i++)
            {
                var unitBinder = this.uiBinder.GetField($"unitGroup_unit{i + 1}") as UIUnitItemBinder;
                var isActive = i < itemCount;
                unitBinder.gameObject.SetActive(isActive);
                if (isActive)
                {
                    var text = unitBinder.txt_name.GetComponent<Text>();
                    text.GetComponent<Text>().text = this._GetItemName(this._itemModels[i]).ToDevStr();
                    var idx = i;
                    this._SetClick(unitBinder.gameObject, () => this._OnClickItem(idx));
                }
            }

            this._SetClick(this.uiBinder.btn_confirm, this._OnBtnConfirmClick);
            this._SetClick(this.uiBinder.btn_refresh, this._OnBtnRefreshClick);
        }

        private void _refreshGold()
        {
            var curGold = this._uiApi.playerApi.curGold;
            this.uiBinder.txt_gold.GetComponent<Text>().text = curGold.ToDevStr();
        }

        private void _OnClickItem(int index)
        {
            this._uiApi.logicApi.directorApi.BuyUnit(index);
        }

        private void _OnBtnConfirmClick()
        {
            // 提交LC, 通知逻辑层确认开始
            this._uiApi.logicApi.directorApi.SubmitEvent(GameLCCollection.LC_GAME_PREPARING_CONFIRM_FIGHT, new GameLCArgs_PreparingConfirmFight());
        }

        private void _OnBtnRefreshClick()
        {
            if (!this._uiApi.logicApi.directorApi.ShuffleBuyableUnits(false)) return;
            this._OnShow();
        }

        private string _GetItemName(GameUnitItemModel itemModel)
        {
            switch (itemModel.entityType)
            {
                case GameEntityType.Role:
                    this._uiApi.rendererApi.roleApi.GetRoleTemplate().TryGet(itemModel.typeId, out var roleModel);
                    return roleModel.roleName;
                case GameEntityType.Skill:
                    return itemModel.typeId.ToString();
                case GameEntityType.Buff:
                    return itemModel.typeId.ToString();
                default:
                    return "未知";
            }
        }

    }
}