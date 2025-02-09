using GamePlay.Bussiness.Logic;
using GamePlay.Bussiness.Renderer;
using UnityEngine.UIElements;

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
            this._uiPanelCom.RefreshHead(this._viewInput.entityType, this._viewInput.entityId);
            this._uiPanelCom.Refersh(this._viewInput.entityType, this._viewInput.entityId);
            this.SetInterval(0.1f, () =>
            {
                this._uiPanelCom.Refersh(this._viewInput.entityType, this._viewInput.entityId);
            });
        }

        protected override void _OnHide()
        {
            base._OnHide();
        }
    }
}