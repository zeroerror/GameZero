using GamePlay.Bussiness.Logic;
using GamePlay.Core;

namespace GamePlay.Bussiness.UI
{
    public class UIActionOptionMgr : UIMgrBase<UIActionOptionMgr>
    {
        public UIActionOptionModel model;

        public UIActionOptionMgr()
        {
            this.model = new UIActionOptionModel();
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
                cost = upgradeCosts.Length > 0 ? upgradeCosts[0] : 0;
            }
            else
            {
                cost = upgradeCosts.Length > selectModel.lv ? upgradeCosts[selectModel.lv] : 0;
            }

            // 检查金币是否足够
            if (!UIPlayerMgr.Instance.IsGoldEnough(cost))
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