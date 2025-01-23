using System;
using System.Collections.Generic;
using GamePlay.Bussiness.Logic;
using GamePlay.Core;
using UnityEngine.UI;
namespace GamePlay.Bussiness.UI
{
    public struct UIActionOptionMainViewInput
    {
        public List<GameActionOptionModel> optionModels;
        public Action<int> onChooseOption;

        public UIActionOptionMainViewInput(List<GameActionOptionModel> optionModels, Action<int> onChooseOption)
        {
            this.optionModels = optionModels;
            this.onChooseOption = onChooseOption;
        }
    }

    public class UIActionOptionMainView : UIBase
    {
        public override UILayerType layerType => UILayerType.PopUp;
        public override string uiPkgUrl => "UI/System/ActionOption";
        public override string uiName => "UIActionOptionMainView";
        public UIActionOptionMainViewBinder viewBinder;

        private UIActionOptionMainViewInput _optionViewInput;
        private List<GameActionOptionModel> _optionModels => this._optionViewInput.optionModels;

        protected override void _OnInit()
        {
            this.viewBinder = new UIActionOptionMainViewBinder(this.go);

            this.domainApi.logicApi.directApi.SetTimeScale(0.01f);
            this.domainApi.rendererApi.directApi.SetTimeScale(0.01f);
            this._optionViewInput = (UIActionOptionMainViewInput)this._viewInput.customData;
        }

        protected override void _OnShow()
        {
            var text1 = this.viewBinder.option1.text;
            text1.GetComponent<Text>().text = this._GetOptionDesc(this._optionModels[0]);
            var text2 = this.viewBinder.option2.text;
            text2.GetComponent<Text>().text = this._GetOptionDesc(this._optionModels[1]);
            var text3 = this.viewBinder.option3.text;
            text3.GetComponent<Text>().text = this._GetOptionDesc(this._optionModels[2]);

            this._AddClick(this.viewBinder.option1.gameObject, () => this._OnClickOption(0));
            this._AddClick(this.viewBinder.option2.gameObject, () => this._OnClickOption(1));
            this._AddClick(this.viewBinder.option3.gameObject, () => this._OnClickOption(2));
        }

        private string _GetOptionDesc(GameActionOptionModel option)
        {
            var desc = option.desc + "\n";
            option.actionIds?.Foreach(actionId =>
            {
                if (!this.domainApi.rendererApi.actionApi.TryGetModel(actionId, out var action)) return;
                desc += action.desc;
            });
            return desc;
        }

        private void _OnClickOption(int index)
        {
            var option = this._optionModels[index];
            var lv = UIActionOptionMgr.Instance.ChooseOption(option);
            if (lv == 0)
            {
                return;
            }

            var playerCampId = GameRoleCollection.PLAYER_ROLE_CAMP_ID;
            this.domainApi.logicApi.actionApi.DoActionOption(option.typeId, playerCampId);
            this._Close();
            this.domainApi.logicApi.directApi.SetTimeScale(1);
            this.domainApi.rendererApi.directApi.SetTimeScale(1);

            var input = (UIActionOptionMainViewInput)this._viewInput.customData;
            input.onChooseOption?.Invoke(option.typeId);
        }
    }
}