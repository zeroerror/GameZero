using System.Collections.Generic;
using GamePlay.Bussiness.Logic;
using GamePlay.Core;
using GamePlay.Infrastructure;

namespace GamePlay.Bussiness.UI
{
    public class UIActionOptionDomain : UISystemDomainBase, UIActionOptionDomainApi
    {
        public UIActionOptionModel model { get; private set; }

        private List<GameActionOptionModel> _actionOptions;

        public UIActionOptionDomain()
        {
            this.model = new UIActionOptionModel();
        }

        protected override void _BindEvents()
        {
            this._context.BindRC(GameDirectorRCCollection.RC_GAME_DIRECTOR_STATE_ENTER_FIGHT_PREPARING, this._OnStateEnterFightPreparing);
        }

        protected override void _UnbindEvents()
        {
            this._context.UnbindRC(GameDirectorRCCollection.RC_GAME_DIRECTOR_STATE_ENTER_FIGHT_PREPARING, this._OnStateEnterFightPreparing);
        }

        private void _OnStateEnterFightPreparing(object args)
        {
            var rcArgs = (GameDirectorRCArgs_StateEnterFightPreparing)args;
            this._actionOptions = rcArgs.actionOptions;
            var viewInput = new UIActionOptionMainViewInput(this._actionOptions);
            this.OpenUI<UIActionOptionMainView>(new UIViewInput(viewInput));
        }

        /// <summary>
        /// 选择选项, 成功时返回当前等级, 失败时返回0
        /// </summary>
        public int ChooseOption(GameActionOptionModel option)
        {
            int cost;
            var upgradeCosts = option.upgradeCosts;
            if (!this.model.TryGetOption(option, out var selectModel))
            {
                cost = upgradeCosts?.Length > 0 ? upgradeCosts[0] : 0;
            }
            else
            {
                cost = upgradeCosts?.Length > selectModel.lv ? upgradeCosts[selectModel.lv] : 0;
            }

            // 检查金币是否足够
            var playerApi = this._context.uiApi.playerApi;
            if (!playerApi.IsGoldEnough(cost))
            {
                GameLogger.DebugLog("金币不足");
                return 0;
            }

            // 检查是否已经达到最大等级
            if (selectModel != null && selectModel.lv >= option.maxLv)
            {
                GameLogger.DebugLog("已达到最大等级");
                return 0;
            }

            if (selectModel == null)
            {

                selectModel = new UIActionOptionSelectModel
                {
                    option = option,
                };
                this.model.Add(selectModel);
            }

            selectModel.lv += 1;
            return selectModel.lv;
        }
    }
}