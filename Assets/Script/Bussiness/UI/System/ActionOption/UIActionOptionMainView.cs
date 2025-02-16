using System.Collections.Generic;
using GamePlay.Bussiness.Logic;
using GamePlay.Core;
using UnityEngine;
namespace GamePlay.Bussiness.UI
{
    public struct UIActionOptionMainViewInput
    {
        public List<GameActionOptionModel> optionModels;

        public UIActionOptionMainViewInput(List<GameActionOptionModel> optionModels)
        {
            this.optionModels = optionModels;
        }
    }

    public class UIActionOptionMainView : UIBase
    {
        public override UILayerType layerType => UILayerType.PopUp;
        public override string uiPkgUrl => "UI/System/ActionOption";
        public override string uiName => "UIActionOptionMainView";
        public UIActionOptionMainViewBinder uiBinder;

        private UIActionOptionMainViewInput _optionViewInput;
        private List<GameActionOptionModel> _optionModels => this._optionViewInput.optionModels;

        protected override void _OnInit()
        {
            this.uiBinder = new UIActionOptionMainViewBinder(this.go);

            this._optionViewInput = (UIActionOptionMainViewInput)this._uiInput.customData;
        }

        protected override void _BindEvents()
        {
            base._BindEvents();
            this._uiApi.directorApi.BindKeyAction(KeyCode.Alpha1, this._OnClickOption0);
            this._uiApi.directorApi.BindKeyAction(KeyCode.Alpha2, this._OnClickOption1);
            this._uiApi.directorApi.BindKeyAction(KeyCode.Alpha3, this._OnClickOption2);
        }

        protected override void _UnbindEvents()
        {
            base._UnbindEvents();
            this._uiApi.directorApi.UnbindKeyAction(KeyCode.Alpha1, this._OnClickOption0);
            this._uiApi.directorApi.UnbindKeyAction(KeyCode.Alpha2, this._OnClickOption1);
            this._uiApi.directorApi.UnbindKeyAction(KeyCode.Alpha3, this._OnClickOption2);
        }

        protected override void _OnShow()
        {
            for (var i = 0; i < this._optionModels.Count; i++)
            {
                var optionBinder = this.uiBinder.GetField($"optionGroup_option{i + 1}") as UIOptionBinder;
                optionBinder.txt_content.text = this._GetOptionContent(this._optionModels[i]).ToDevStr();
                optionBinder.txt_title.text = this._optionModels[i].desc.ToDevStr();

                var optionIndex = i;
                this.SetClick(optionBinder.gameObject, () => this._OnClickOption(optionIndex));
                GameLogger.DebugLog($"选项{i + 1}: {this._GetOptionContent(this._optionModels[i])}");
            }
        }

        private string _GetOptionContent(GameActionOptionModel option)
        {
            var desc = "";
            option.actionIds?.Foreach(actionId =>
            {
                if (!this._uiApi.rendererApi.actionApi.TryGetModel(actionId, out var action)) return;
                desc += action.desc;
                desc += "\n";
            });
            return desc;
        }

        private void _OnClickOption(int index)
        {
            var option = this._optionModels[index];
            var actionOptionApi = this._uiApi.actionOptionApi;
            var lv = actionOptionApi.ChooseOption(option);
            if (lv == 0)
            {
                return;
            }

            this._Close();

            // 提交LC
            var lcArgs = new GameLCArgs_ActionOptionSelected
            {
                optionId = option.typeId,
            };
            this._uiApi.logicApi.directorApi.SubmitEvent(GameLCCollection.LC_GAME_ACTION_OPTION_SELECTED, lcArgs);
        }
        private void _OnClickOption0() => this._OnClickOption(0);
        private void _OnClickOption1() => this._OnClickOption(1);
        private void _OnClickOption2() => this._OnClickOption(2);
    }
}