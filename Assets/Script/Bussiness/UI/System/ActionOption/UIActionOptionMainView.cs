using System;
using System.Collections.Generic;
using GamePlay.Bussiness.Logic;
using GamePlay.Core;
using UnityEngine;
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
        public UIActionOptionMainViewBinder uiBinder;

        private UIActionOptionMainViewInput _optionViewInput;
        private List<GameActionOptionModel> _optionModels => this._optionViewInput.optionModels;

        protected override void _OnInit()
        {
            this.uiBinder = new UIActionOptionMainViewBinder(this.go);

            this._uiApi.logicApi.directorApi.SetTimeScale(0.01f);
            this._uiApi.rendererApi.directorApi.SetTimeScale(0.01f);
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
                var optionBinder = this.uiBinder.GetField($"option{i + 1}") as UIOptionBinder;
                var text = optionBinder.text;
                text.GetComponent<Text>().text = this._GetOptionDesc(this._optionModels[i]).ToDevStr();
                var optionIndex = i;
                this._SetClick(optionBinder.gameObject, () => this._OnClickOption(optionIndex));
            }
        }

        private string _GetOptionDesc(GameActionOptionModel option)
        {
            var desc = option.desc + "\n";
            option.actionIds?.Foreach(actionId =>
            {
                if (!this._uiApi.rendererApi.actionApi.TryGetModel(actionId, out var action)) return;
                desc += action.desc;
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
            this._uiApi.logicApi.directorApi.SetTimeScale(1);
            this._uiApi.rendererApi.directorApi.SetTimeScale(1);

            var input = (UIActionOptionMainViewInput)this._uiInput.customData;
            input.onChooseOption?.Invoke(option.typeId);
        }
        private void _OnClickOption0() => this._OnClickOption(0);
        private void _OnClickOption1() => this._OnClickOption(1);
        private void _OnClickOption2() => this._OnClickOption(2);
    }
}