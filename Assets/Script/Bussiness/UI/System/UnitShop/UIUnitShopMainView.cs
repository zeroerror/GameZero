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
        public override UILayerType layerType => UILayerType.Main;
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
            this._uiApi.directorApi.BindKeyAction(KeyCode.G, () => this._OnClickItem(0));
            this._uiApi.directorApi.BindKeyAction(KeyCode.H, () => this._OnClickItem(1));
            this._uiApi.directorApi.BindKeyAction(KeyCode.J, () => this._OnClickItem(2));
            this._uiApi.directorApi.BindKeyAction(KeyCode.K, () => this._OnClickItem(3));
            this._uiApi.directorApi.BindKeyAction(KeyCode.L, () => this._OnClickItem(4));
        }

        protected override void _UnbindEvents()
        {
            base._UnbindEvents();
            this._uiApi.directorApi.UnbindKeyAction(KeyCode.G, () => this._OnClickItem(0));
            this._uiApi.directorApi.UnbindKeyAction(KeyCode.H, () => this._OnClickItem(1));
            this._uiApi.directorApi.UnbindKeyAction(KeyCode.J, () => this._OnClickItem(2));
            this._uiApi.directorApi.UnbindKeyAction(KeyCode.K, () => this._OnClickItem(3));
            this._uiApi.directorApi.UnbindKeyAction(KeyCode.L, () => this._OnClickItem(4));
        }

        protected override void _OnShow()
        {
            var text1 = this.viewBinder.unitGroup_unit1.txt_name;
            text1.GetComponent<Text>().text = this._GetItemName(this._itemModels[0]);
            var text2 = this.viewBinder.unitGroup_unit2.txt_name;
            text2.GetComponent<Text>().text = this._GetItemName(this._itemModels[1]);
            var text3 = this.viewBinder.unitGroup_unit3.txt_name;
            text3.GetComponent<Text>().text = this._GetItemName(this._itemModels[2]);
            this._AddClick(this.viewBinder.unitGroup_unit1.gameObject, () => this._OnClickItem(0));
            this._AddClick(this.viewBinder.unitGroup_unit2.gameObject, () => this._OnClickItem(1));
            this._AddClick(this.viewBinder.unitGroup_unit3.gameObject, () => this._OnClickItem(2));
            this._AddClick(this.viewBinder.unitGroup_unit3.gameObject, () => this._OnClickItem(3));
            this._AddClick(this.viewBinder.unitGroup_unit3.gameObject, () => this._OnClickItem(4));
        }

        private string _GetItemName(GameUnitItemModel itemModel)
        {
            switch (itemModel.entityType)
            {
                case GameEntityType.Role:
                    this._uiApi.rendererApi.roleApi.GetRoleTemplate().TryGet(itemModel.typeId, out var roleModel);
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
            this._uiApi.logicApi.directorApi.BuyUnit(index);
        }
    }
}