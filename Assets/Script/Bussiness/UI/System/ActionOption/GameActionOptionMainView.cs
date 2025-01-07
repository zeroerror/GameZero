using System.Collections.Generic;
using System.Diagnostics;
using GamePlay.Bussiness.Logic;
using GamePlay.Bussiness.Renderer;
using GamePlay.Core;
using UnityEngine;
using UnityEngine.UI;
namespace GamePlay.Bussiness.UI
{
    public class GameUI_ActionOption : GameUIBase
    {
        public override GameUILayerType layerType => GameUILayerType.PopUp;
        public override string uiPkgUrl => "UI/System/ActionOption";
        public override string uiName => "ActionOptionMainView";

        private List<GameActionOptionModel> _optionModels;

        public GameActionOptionMainViewBinder viewBinder;

        protected override void _OnInit()
        {
            this.viewBinder = new GameActionOptionMainViewBinder(this.go);
        }

        protected override void _OnShow()
        {
            this._optionModels = this._getRandomActionOptions(3);
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
            var desc = "";
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
            var playerCampId = GameRoleCollection.PLAYER_ROLE_CAMP_ID;
            this.domainApi.logicApi.actionApi.DoActionOption(option.typeId, playerCampId);
            this._Close();
        }

        private List<GameActionOptionModel> _getRandomActionOptions(int count)
        {
            var actionApi = this.domainApi.logicApi.actionApi;
            var list = actionApi.GetActionOptionModelList();
            var result = new List<GameActionOptionModel>();
            for (int i = 0; i < count; i++)
            {
                var index = GameMath.RandomRange(0, list.Count);
                var model = list[index];
                if (model == null)
                {
                    i--;
                    continue;
                }
                result.Add(model);
                list[index] = null;
            }
            return result;
        }
    }
}