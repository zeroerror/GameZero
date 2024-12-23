using GamePlay.Bussiness.Logic;
using GamePlay.Bussiness.Renderer;
using GamePlay.Core;
using UnityEngine;

namespace GamePlay.Config
{
    [CreateAssetMenu(fileName = "template_buff_", menuName = "游戏玩法/配置/buff模板")]
    public class GameBuffSO : GameSOBase
    {
        [Header("名称")]
        public string buffName;
        [Header("描述")]
        public string desc;
        [Header("刷新类型标记")]
        public GameBuffRefreshFlag refreshFlag;
        [Header("最大层数")]
        public int maxLayer;
        [Header("行为模板列表")]
        public GameActionSO[] actionSOs;
        [Header("条件集模板 - 触发行为")]
        public GameBuffConditionSetEM conditionSetEM_action;
        [Header("条件集模板 - 移除")]
        public GameBuffConditionSetEM conditionSetEM_remove;
        [Header("buff特效")]
        public GameObject vfxPrefab;
        public string vfxPrefabUrl;
        [Header("挂载层级")]
        public GameFieldLayerType vfxLayerType;

        public GameBuffModel ToModel()
        {
            var model = new GameBuffModel(
                typeId,
                refreshFlag,
                maxLayer,
                actionSOs.Map(e => e.typeId),
                conditionSetEM_action.ToModel(),
                conditionSetEM_remove.ToModel()
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
