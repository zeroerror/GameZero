using GamePlay.Bussiness.Logic;
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
        public override UILayerType layerType => UILayerType.PopUp;
        public override string uiPkgUrl => "UI/System/UnitShop";
        public override string uiName => "UIUnitShopMainView";
        public UIUnitShopMainViewBinder viewBinder;

        private UIUnitShopMainViewInput _viewInput;
        private GameUnitItemModel[] _itemModels => this._viewInput.buyableUnits;

        protected override void _OnInit()
        {
            this.viewBinder = new UIUnitShopMainViewBinder(this.go);
            this._viewInput = (UIUnitShopMainViewInput)this._uiInput.customData;
        }

        protected override void _BindEvents()
        {
            base._BindEvents();
            this._domainApi.directApi.BindKeyAction(KeyCode.G, () => this._OnClickItem(0));
            this._domainApi.directApi.BindKeyAction(KeyCode.H, () => this._OnClickItem(1));
            this._domainApi.directApi.BindKeyAction(KeyCode.J, () => this._OnClickItem(2));
            this._domainApi.directApi.BindKeyAction(KeyCode.K, () => this._OnClickItem(3));
            this._domainApi.directApi.BindKeyAction(KeyCode.L, () => this._OnClickItem(4));
        }

        protected override void _UnbindEvents()
        {
            base._UnbindEvents();
            this._domainApi.directApi.UnbindKeyAction(KeyCode.G, () => this._OnClickItem(0));
            this._domainApi.directApi.UnbindKeyAction(KeyCode.H, () => this._OnClickItem(1));
            this._domainApi.directApi.UnbindKeyAction(KeyCode.J, () => this._OnClickItem(2));
            this._domainApi.directApi.UnbindKeyAction(KeyCode.K, () => this._OnClickItem(3));
            this._domainApi.directApi.UnbindKeyAction(KeyCode.L, () => this._OnClickItem(4));
        }

        protected override void _OnShow()
        {
            var text1 = this.viewBinder.unit1.txt_name;
            text1.GetComponent<Text>().text = this._GetItemName(this._itemModels[0]);
            var text2 = this.viewBinder.unit2.txt_name;
            text2.GetComponent<Text>().text = this._GetItemName(this._itemModels[1]);
            var text3 = this.viewBinder.unit3.txt_name;
            text3.GetComponent<Text>().text = this._GetItemName(this._itemModels[2]);
            this._AddClick(this.viewBinder.unit1.gameObject, () => this._OnClickItem(0));
            this._AddClick(this.viewBinder.unit2.gameObject, () => this._OnClickItem(1));
            this._AddClick(this.viewBinder.unit3.gameObject, () => this._OnClickItem(2));
            this._AddClick(this.viewBinder.unit3.gameObject, () => this._OnClickItem(3));
            this._AddClick(this.viewBinder.unit3.gameObject, () => this._OnClickItem(4));
        }

        private string _GetItemName(GameUnitItemModel itemModel)
        {
            switch (itemModel.entityType)
            {
                case GameEntityType.Role:
                    this._domainApi.rendererApi.roleApi.GetRoleTemplate().TryGet(itemModel.typeId, out var roleModel);
                    return "角色" + roleModel.roleName;
                case GameEntityType.Skill:
                    return "技能" + itemModel.typeId;
                case GameEntityType.Buff:
                    return "Buff" + itemModel.typeId;
                default:
                    return "未知";
            }
        }

        private void _OnClickItem(int index)
        {
            this._domainApi.logicApi.directApi.BuyUnit(index);
        }
    }
}