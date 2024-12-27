using GamePlay.Bussiness.Logic;
using GamePlay.Bussiness.Renderer;
using GamePlay.Core;
using UnityEngine;

namespace GamePlay.Config
{
    [CreateAssetMenu(fileName = "template_buff_", menuName = "游戏玩法/配置/buff模板")]
    public class GameBuffSO : GameSOBase
    {
        public string buffName;
        public string desc;
        public GameBuffRefreshFlag refreshFlag;
        public int maxLayer;
        public GameActionSO[] actionSOs;
        public GameBuffConditionSetEM conditionSetEM_action;
        public GameBuffConditionSetEM conditionSetEM_remove;
        public GameObject vfxPrefab;
        public string vfxPrefabUrl;
        public GameFieldLayerType vfxLayerType;
        public GameBuffAttributeEM[] attributeEMs;

        public GameBuffModel ToModel()
        {
            var model = new GameBuffModel(
                typeId,
                refreshFlag,
                maxLayer,
                actionSOs.Map(e => e.typeId),
                conditionSetEM_action.ToModel(),
                conditionSetEM_remove.ToModel(),
                attributeEMs.Map(e => e.ToModel())
            );
            return model;
        }

        public GameBuffModelR ToModelR()
        {
            var model = new GameBuffModelR(
                typeId,
                refreshFlag,
                maxLayer,
                actionSOs.Map(e => e.typeId),
                conditionSetEM_action.ToModel(),
                conditionSetEM_remove.ToModel(),
                vfxPrefabUrl,
                vfxLayerType
            );
            return model;
        }
    }
}
