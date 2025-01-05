namespace GamePlay.Bussiness.UI
{
    public class GameUI_ActionOption : GameUIBase
    {
        public override GameUILayerType layerType => GameUILayerType.PopUp;
        public override string uiPkgUrl => "UI/System/ActionOption";
        public override string uiName => "ActionOptionMainView";

        protected override void _OnDestroy()
        {
        }

        protected override void _OnHide()
        {
            throw new System.NotImplementedException();
        }

        protected override void _OnInit()
        {
        }

        protected override void _OnShow()
        {
        }
    }
}