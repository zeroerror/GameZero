using System.Collections.Generic;
using GamePlay.Bussiness.Renderer;
using GamePlay.Core;
using UnityEngine.UI;
namespace GamePlay.Bussiness.UI
{
    public class GameUI_ActionOption : GameUIBase
    {
        public override GameUILayerType layerType => GameUILayerType.PopUp;
        public override string uiPkgUrl => "UI/System/ActionOption";
        public override string uiName => "ActionOptionMainView";

        public GameActionOptionMainViewBinder viewBinder;

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
            var buffs = this._getRandomBuffs(3);

            var text1 = this.viewBinder.option1.text;
            text1.GetComponent<Text>().text = buffs[0].desc;
            var text2 = this.viewBinder.option2.text;
            text2.GetComponent<Text>().text = buffs[1].desc;
            var text3 = this.viewBinder.option3.text;
            text3.GetComponent<Text>().text = buffs[2].desc;
        }

        private List<GameBuffModelR> _getRandomBuffs(int count)
        {
            var buffApi = this.domainApi.rendererApi.buffApi;
            var list = buffApi.GetBuffModelList();
            var result = new List<GameBuffModelR>();
            for (int i = 0; i < count; i++)
            {
                var index = GameMath.RandomRange(0, list.Count);
                var model = list[index];
                if (model == null) continue;
                result.Add(model);
                list[index] = null;
            }
            return result;
        }
    }
}