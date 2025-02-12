using GamePlay.Bussiness.Logic;

namespace GamePlay.Bussiness.UI
{
    public struct UIUnitDetailMainViewInput
    {
        public GameEntityType entityType;
        public int entityId;
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
            this._uiPanelCom = new UIPanelCom(this.viewBinder.panelCom, this._uiApi);
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
            this.Refresh(this._viewInput);
        }

        public void Refresh(UIUnitDetailMainViewInput viewInput)
        {
            this._uiPanelCom.RefreshHead(viewInput.entityType, viewInput.entityId);
            this._uiPanelCom.Refersh(viewInput.entityType, viewInput.entityId);
            if (this._timerId != 0)
            {
                this.RemoveTimer(this._timerId);
                this._timerId = 0;
            }
            this._timerId = this.SetInterval(0.1f, () =>
            {
                this._uiPanelCom.Refersh(viewInput.entityType, viewInput.entityId);
            });
        }
        private int _timerId;

        protected override void _OnHide()
        {
            base._OnHide();
        }
    }
}