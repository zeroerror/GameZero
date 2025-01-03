using GamePlay.Bussiness.Logic;
using GamePlay.Core;
using UnityEngine;

namespace GamePlay.Config
{
    [CreateAssetMenu(fileName = "template_action_option_", menuName = "游戏玩法/配置/行为选项模板")]
    public class GameActionOptionSO : GameSOBase
    {
        [Header("质量")]
        public GameActionOptionQuality quality;
        [Header("最大等级")]
        public int maxLv;
        [Header("行为列表")]
        public GameActionSO[] actionSOs;

        public GameActionOptionModel ToModel()
        {
            if (actionSOs == null || actionSOs.Length == 0)
            {
                return null;
            }
            var model = new GameActionOptionModel(
                this.typeId,
                this.quality,
                this.maxLv,
                this.actionSOs.Map(so => so.typeId)
            );
            return model;
        }
    }
}