using GamePlay.Bussiness.Logic;
using GamePlay.Bussiness.Renderer;
using UnityEngine.UIElements;

namespace GamePlay.Bussiness.UI
{
    public struct UIUnitDetailMainViewInput
    {
        public GameEntityBase clickEntity;
    }

    public class UIUnitDetailMainView : UIBase
    {
        public override UILayerType layerType => UILayerType.PopUp;
        public override string uiPkgUrl => "UI/System/UnitDetail";
        public override string uiName => "UIUnitDetailMainView";
        public UIUnitDetailMainViewBinder viewBinder;
        private UIUnitDetailMainViewInput _viewInput;

        private UIPanelCom _uiPanelCom;

        protected override void _OnInit()
        {
            this.viewBinder = new UIUnitDetailMainViewBinder(this.go);
            this._viewInput = (UIUnitDetailMainViewInput)this._uiInput.customData;
            this._uiPanelCom = new UIPanelCom(this.viewBinder.panelCom, this._viewInput.clickEntity);
        }

        protected override void _BindEvents()
        {
            base._BindEvents();
        }

        protected override void _UnbindEvents()
        {
            base._UnbindEvents();
        }

        protected override void _OnShow()
        {
            base._OnShow();
            this._RefreshPanelCom();
            this.SetInterval(0.1f, () =>
            {
                this._RefreshPanelCom();
            });
        }

        private void _RefreshPanelCom()
        {
            var entity = this._uiApi.logicApi.directorApi.FindEntity(this._viewInput.clickEntity.idCom.ToArgs());
            var attributes = entity?.attributeCom.ToArgs().attributes;
            var baseAttributes = entity?.baseAttributeCom.ToArgs().attributes;
            this._uiPanelCom.Refersh(attributes, baseAttributes);
        }

        protected override void _OnHide()
        {
            base._OnHide();
        }
    }
}