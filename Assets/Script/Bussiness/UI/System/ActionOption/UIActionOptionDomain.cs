using System;
using GamePlay.Bussiness.Logic;
using GamePlay.Core;

namespace GamePlay.Bussiness.UI
{
    public class UIActionOptionDomain : UISystemDomainBase, UIActionOptionDomainApi
    {
        public UIActionOptionModel model { get; private set; }

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

            // 2s后打开行为选项界面
            var actionOptions = rcArgs.actionOptions;
            this._context.cmdBufferService.AddDelayCmd(2, () =>
            {
                var viewInput = new UIActionOptionMainViewInput(actionOptions);
                this.OpenUI<UIActionOptionMainView>(new UIViewInput(viewInput));
            });
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
            var playerApi = this._context.domainApi.playerApi;
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