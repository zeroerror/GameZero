using GamePlay.Bussiness.Logic;
using GamePlay.Core;
using UnityEngine;

namespace GamePlay.Config
{
    [CreateAssetMenu(fileName = "template_action_option_", menuName = "游戏玩法/配置/行为选项模板")]
    public class GameActionOptionSO : GameSOBase
    {
        [Header("是否禁用")]
        public bool disable;

        [Header("质量")]
        public GameActionOptionQuality quality;
        [Header("描述")]
        public string desc;
        [Header("最大等级")]
        public int maxLv;
        [Header("行为列表")]
        public GameActionSO[] actionSOs;
        [Header("升级金币消耗列表")]
        public int[] upgradeCosts;

        public GameActionOptionModel ToModel()
        {
            if (actionSOs == null || actionSOs.Length == 0)
            {
                return null;
            }
            var model = new GameActionOptionModel(
                this.typeId,
                this.desc,
                this.quality,
                this.maxLv,
                this.actionSOs.Map(so => so.typeId),
                this.upgradeCosts
            );
            return model;
        }
    }
}